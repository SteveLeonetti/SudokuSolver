using Assets.Objects;
using System;
using System.Collections.Generic;

/// <summary>
/// 1. Check single boxes if they can only be 1 value. (Repeat until no progress)
/// 2. Check sets (row, column, box) if a single box in that set can be a certain value. (Regress to Stage 1 if successful)
/// 3. Check for advanced techniques within a set (row, column, box). (Regress to Stage 1 if successful)
/// </summary>
public static class Solver
{
	static int setSize;
	static SudokuBoard board;
	static OrderedSet<int> sudokuNumbers;

	static Dictionary<string, OrderedSet<int>> attemptedGuesses;
	static OrderedSet<SNode> sNodesWithLeastPossibles;
	static bool guessPlaced = false;
	static string backup;
	//static SudokuBoard backup;
	static SNode guessedNode;
	static int guessedValue;
	static int failCount = 0;

	//
	static StackSet<Assignment> untested = new StackSet<Assignment>();
	static StackSet<Assignment> tested = new StackSet<Assignment>();

	static StackSet<Assignment> testing = new StackSet<Assignment>();
	//

	static Controller controller;

	/// <summary>
	/// Solves the puzzle as best it can.  This is the method portraying the logical order of solving.
	/// </summary>
	/// <param name="s"></param>
	/// <param name="c"></param>
	/// <returns>integer type board</returns>
	public static int[,] Solve(SNode[,] s, Controller c)
	{
		//untested.Remove(tested[0]);
		controller = c;

		Load(s); // Loads initial state with predetermined obvious "not-possibles"

		while (!IsSolved())
		{
			// Precedence 0: Solve spaces when only 1 value is possible.
			if (NakedSingle())
			{
				failCount = 0;
				continue;
			}

			// Precedence 1: Remove some possibles with this solve technique.  Line/Box Reduction.
			if (LineBoxReduction())
			{
				failCount = 0;
				continue;
			}

			// Precedence 2: Remove some possibles with this solve technique.  Naked, Single, Pair, Triplet, Quadruplet, etc...
			if (SetLoop())
			{
				failCount = 0;
				continue;
			}

			// Precedence 3: Remove some possibles with this solve technique.  XWing[Row->Col] or XWing[Col->Row]
			if (XWingLoop())
			{
				failCount = 0;
				continue;
			}

			// Precedence 4: Guess a number into a SNode.  Hope that it causes contradiction or completed the puzzle.
			//if (!guessPlaced)
			//{
			//    BackupSudokuBoard();
			//    GuessValue();
			//}
			//else
			//{
			//    if (Contradiction())
			//    {
			//        attemptedGuesses.Clear();
			//
			//        RetractSudokuBoardState();
			//
			//        guessedNode = board.Sudoku[guessedNode.columnIndex, guessedNode.rowIndex];
			//
			//        guessedNode.Possibles.Remove(guessedValue);
			//    }
			//    else    // we didn't gain any knowledge....try a different number on same SNode or different SNode entirely.
			//    {
			//        string coord = "" + guessedNode.columnIndex + guessedNode.rowIndex;
			//
			//        if (!attemptedGuesses.ContainsKey(coord))
			//            attemptedGuesses.Add(coord, new OrderedSet<int>());
			//
			//        attemptedGuesses[coord].Add(guessedValue);
			//
			//        RetractSudokuBoardState();
			//        GuessValue();
			//    }
			//}

			if (failCount > 2)
				return SNode.ConvertToOutput(board);

			failCount++;
		}

		if (controller != null)
		{
			string output = "Puzzle Solved!";
			controller.PrintStatement(output);
		}

		return SNode.ConvertToOutput(board);
	} // Solve()


	/// <summary>
	/// Loads data about sudoku board.
	/// </summary>
	/// <param name="s">Sudoku Board</param>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "<Pending>")]
	private static void Load(SNode[,] s)
	{
		setSize = s.GetLength(0);
		sudokuNumbers = new OrderedSet<int>();
		attemptedGuesses = new Dictionary<string, OrderedSet<int>>();

		for (int i = 1; i <= setSize; i++)
			sudokuNumbers.Add(i);

		board = new SudokuBoard(s);

		foreach (SNode sNode in board.Sudoku)
			CheckImpossibles(sNode);
	} // Load()

	#region Check
	/// <summary>
	/// Checks nodes versus it's containing Row, Column, and Box to see what numbers it CAN'T be.
	/// Assigns false boolean to spaces in the bool[] named possibles for said node.
	/// </summary>
	private static void CheckImpossibles(SNode sNode)
	{
		if (sNode.IsSolved)
			sNode.ApplyValue(sNode.Value);
		else
		{
			#region Row
			foreach (SNode relatedNode in sNode.Row)
			{
				if (relatedNode.Value > 0)
				{
					if (sNode.Possibles.Contains(relatedNode.Value))
						sNode.Possibles.Remove(relatedNode.Value);
				}
			}
			#endregion

			#region Column
			foreach (SNode relatedNode in sNode.Column)
			{
				if (relatedNode.Value > 0)
				{
					if (sNode.Possibles.Contains(relatedNode.Value))
						sNode.Possibles.Remove(relatedNode.Value);
				}
			}
			#endregion

			#region Box
			foreach (SNode relatedNode in sNode.Box)
			{
				if (relatedNode.Value > 0)
				{
					if (sNode.Possibles.Contains(relatedNode.Value))
						sNode.Possibles.Remove(relatedNode.Value);
				}
			}
			#endregion
		}
	} // CheckImpossibles()

	#region Guessing Logic
	/// <summary>
	/// Stores data of the Sudoku Board while it's still 100% accurate with no guesses.
	/// </summary>
	//private static void BackupSudokuBoard()
	//{
	//    SNode[,] b = new SNode[board.Columns.Count, board.Rows.Count];
	//
	//    for (int row = 0; row < board.Rows.Count; row++)
	//    {
	//        for (int column = 0; column < board.Columns.Count; column++)
	//            b[column, row] = board.Sudoku[column, row].DeepCopy();
	//    }
	//
	//    backup = new SudokuBoard(b);
	//} // BackupSudokuBoard()

	private static void BackupSudokuBoard()
	{
		backup = board.ToString();
	}

	/// <summary>
	/// Retrieve data of Sudoku Board when it was at 100% accuracy with no guesses.
	/// </summary>
	//private static void RetractSudokuBoardState()
	//{
	//    board = new SudokuBoard(backup.Sudoku);
	//
	//    guessPlaced = false;
	//} //RetractSudokuBoardState()

		private static void RetractSudokuBoardState()
	{
		board = new SudokuBoard(backup);

		

		guessPlaced = false;
	}

	/// <summary>
	/// Finds the first SNode with the least number of possible numbers and guesses one of it's remaining numbers.
	/// </summary>
	private static void GuessValue()
	{
		// Find a SNode with the least possibles, make a guess, store the currently guessed SNode and the value attempted.
		#region List SNodes with least amount of possibles
		int possCount = 2;
		sNodesWithLeastPossibles = new OrderedSet<SNode>();

		while (possCount <= setSize)
		{
			foreach (SNode sNode in board.Sudoku)
			{
				if (sNode.PossiblesCount() == possCount)
					sNodesWithLeastPossibles.Add(sNode);
			}

			if (sNodesWithLeastPossibles.Count == 0)
				possCount++;
			else
				break;
		}
		#endregion

		#region Pick SNode from List
		foreach (SNode sNode in sNodesWithLeastPossibles)
		{
			guessedNode = sNode;
			guessedValue = 0;
			string coord = "" + guessedNode.columnIndex + guessedNode.rowIndex;

			foreach (int value in sNode.Possibles)
			{
				if (!attemptedGuesses.ContainsKey(coord) || !attemptedGuesses[coord].Contains(value))
				{
					guessedValue = value;
					break;
				}

				if (guessedValue > 0)
					break;
			}
		}
		#endregion

		guessedNode.ApplyValue(guessedValue);

		guessPlaced = true;
	} // GuessValue()

	/// <summary>
	/// Checks if an SNode can't be any numbers.
	/// </summary>
	/// <returns></returns>
	private static bool Contradiction()
	{
		foreach (SNode sNode in board.Sudoku)
		{
			if (!sNode.IsSolved && sNode.PossiblesCount() == 0)
				return true;
		}

		return false;
	} // Contradiction()
	#endregion

	/// <summary>
	/// Check for Naked Singles: if any plug the number into the SNode.
	/// </summary>
	/// <returns></returns>
	private static bool NakedSingle()
	{
		bool progress = false;

		foreach (SNode sNode in board.Sudoku)
		{
			int value = 0;

			if (sNode.PossiblesCount() == 1)
			{
				value = sNode.Possibles[0];
				sNode.ApplyValue(value);
				progress = true;
			}

			if (progress)
			{
				if (controller != null)
				{
					string output = "Updated board with Naked Single Solve. " + value + " was used at Node Index {" + (sNode.columnIndex + 1) + ", " + (sNode.rowIndex + 1) + "}.";
					controller.PrintStatement(output);
				}
			}
		}

		return progress;
	} // NakedSingle()

	/// <summary>
	/// Finds Pointing Pairs.
	/// </summary>
	/// <returns></returns>
	private static bool PointingPair()
	{
		foreach (Box box in board.Boxes)
		{
			SudokuSet shrunkenBox = box.Trim();

			foreach (int value in sudokuNumbers)
			{
				OrderedSet<Row> rows = new OrderedSet<Row>();
				OrderedSet<Column> cols = new OrderedSet<Column>();

				foreach (SNode sNode in shrunkenBox)
				{
					if (sNode.Possibles.Contains(value))
					{
						rows.Add(sNode.Row);
						cols.Add(sNode.Column);
					}
				}

				bool boardUpdated = false;

				if (rows.Count == 1)
				{
					Row row = rows[0];
					foreach (SNode sNode in row)
					{
						if (sNode.Box != box && sNode.Possibles.Contains(value))
						{
							sNode.Possibles.Remove(value);
							boardUpdated = true;
						}
					}
				}

				if (cols.Count == 1)
				{
					Column column = cols[0];
					foreach (SNode sNode in column)
					{
						if (sNode.Box != box && sNode.Possibles.Contains(value))
						{
							sNode.Possibles.Remove(value);
							boardUpdated = true;
						}
					}
				}

				if (boardUpdated)
				{
					if (controller != null)
					{
						string output = "Updated board with Pointing Pair Reduction. " + value + " was used.";
						controller.PrintStatement(output);
					}

					return true;
				}
			}
		}

		return false;
	} // PointingPair()    ** COVERED BY NAKED PAIR.....THIS TECHNIQUE IS STUPID!!!!!

	/// <summary>
	/// Finds Line/Box Reductions.
	/// </summary>
	/// <returns></returns>
	private static bool LineBoxReduction()
	{
		bool boardUpdated = false;

		List<SNode> shared;
		List<SNode> lineOnly;
		List<SNode> boxOnly;

		foreach (Box b in board.Boxes)
		{
			bool sharedPossible;
			bool linePossible;
			bool boxPossible;

			foreach (Column c in b.linkedColumns)
			{
				shared = new List<SNode>();
				lineOnly = new List<SNode>();
				boxOnly = new List<SNode>();

				# region Fill Sets
				foreach (SNode n in b)
				{
					if (c.Contains(n))
						shared.Add(n);
					else
						boxOnly.Add(n);
				}
				foreach (SNode n in c)
				{
					if (b.Contains(n))
						shared.Add(n);
					else
						lineOnly.Add(n);
				}
				#endregion

				#region Iterate through and check
				foreach (int value in sudokuNumbers)
				{
					sharedPossible = false;
					linePossible = false;
					boxPossible = false;

					#region Check which sets contain possibility of using a Sudoku Number
					foreach (SNode node in shared)
					{
						if (node.Possibles.Contains(value))
							sharedPossible = true;
					}

					foreach (SNode node in lineOnly)
					{
						if (node.Possibles.Contains(value))
							linePossible = true;
					}

					foreach (SNode node in boxOnly)
					{
						if (node.Possibles.Contains(value))
							boxPossible = true;
					}
					#endregion

					#region Check truthfulness
					if (sharedPossible)
					{
						if (linePossible && !boxPossible)
						{
							// Remove Possible from lineOnly nodes.
							foreach (SNode node in lineOnly)
							{
								node.Possibles.Remove(value);
							}

							boardUpdated = true;
							break;
						}
						else if (boxPossible && !linePossible)
						{
							// Remove Possibles from boxOnly nodes.
							foreach (SNode node in boxOnly)
							{
								node.Possibles.Remove(value);
							}

							boardUpdated = true;
							break;
						}
					}
					#endregion

					if (boardUpdated)
					{
						if (controller != null)
						{
							string output = "Updated board with Line/Box Reduction. " + value + " was used on Box " + b.Index + " with Column " + c.Index + ".";
							controller.PrintStatement(output);
						}

						return true;
					}
				}
			}
			#endregion

			foreach (Row r in b.linkedRows)
			{
				shared = new List<SNode>();
				lineOnly = new List<SNode>();
				boxOnly = new List<SNode>();

				#region Fill Sets
				foreach (SNode n in b)
				{
					if (r.Contains(n))
						shared.Add(n);
					else
						boxOnly.Add(n);
				}
				foreach (SNode n in r)
				{
					if (b.Contains(n))
						shared.Add(n);
					else
						lineOnly.Add(n);
				}
				#endregion

				#region Iterate through and check
				foreach (int value in sudokuNumbers)
				{
					sharedPossible = false;
					linePossible = false;
					boxPossible = false;

					#region Check which sets contain possibility of using a Sudoku Number
					foreach (SNode node in shared)
					{
						if (node.Possibles.Contains(value))
							sharedPossible = true;
					}

					foreach (SNode node in lineOnly)
					{
						if (node.Possibles.Contains(value))
							linePossible = true;
					}

					foreach (SNode node in boxOnly)
					{
						if (node.Possibles.Contains(value))
							boxPossible = true;
					}
					#endregion

					#region Check truthfulness
					if (sharedPossible)
					{
						if (linePossible && !boxPossible)
						{
							// Remove Possible from lineOnly nodes.
							foreach (SNode node in lineOnly)
							{
								node.Possibles.Remove(value);
							}

							boardUpdated = true;
							break;
						}
						else if (boxPossible && !linePossible)
						{
							// Remove Possibles from boxOnly nodes.
							foreach (SNode node in boxOnly)
							{
								node.Possibles.Remove(value);
							}

							boardUpdated = true;
							break;
						}
					}
					#endregion

					if (boardUpdated)
					{
						if (controller != null)
						{
							string output = "Updated board with Line/Box Reduction. " + value + " was used on Box " + b.Index + " with Row " + r.Index +  ".";
							controller.PrintStatement(output);
						}

						return true;
					}
				}
				#endregion
			}
		}
		return false;
	} // LineBoxReduction()

	/// <summary>
	/// Looping structure for solving Naked Sets.  Uses CombinationNoRepitition to check all chances.
	/// </summary>
	/// <returns></returns>
	private static bool SetLoop()
	{
		int[] easinessRating = { 8, 2, 7, 3, 6, 4, 5 };
		/*
			Variable of easinessRating depends on how long the loops may need to run
			1. Easiest:         8                               (~100 loops)
			2. Moderate:    2 and 7                     (~1300 loops)
			3. Hard:            3 and 6                     (~7000 loops)
			4. Hardest:       4 and 5                     (~16000 loops)
		*/

		for (int place = 0; place < easinessRating.Length; place++)
		{
			if (NakedHiddenSet(easinessRating[place]))
				return true;
		}

		return false;
	} // SetLoop()

	/// <summary>
	/// Using the size provided from the parameter, finds Naked sets of that size.
	/// IE: Size 1 is a Naked Single, Size 3 is a Naked Triplet, and Size 8 is a Naked Octet (AKA Hidden Single)
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	private static bool NakedHiddenSet(int size)
	{
		foreach (SudokuSet set in board.Sets)
		{
			int shrunkenSetSize = set.UnsolvedValues.Count;
			int numCount = shrunkenSetSize - size;
			if (numCount <= 0 || shrunkenSetSize == 0)
				continue;

			SudokuSet shrunkenSet = set.Trim();

			#region Nodes
			CombinationNoRepetition<SNode> proposedSetOfSudokuNodes = new CombinationNoRepetition<SNode>(shrunkenSet, size);
			#endregion

			#region Numbers
			CombinationNoRepetition<int> proposedSudokuNumbers = new CombinationNoRepetition<int>(set.UnsolvedValues, numCount);
			#endregion

			do
			{
				OrderedSet<SNode> chosenSudokuNodes = proposedSetOfSudokuNodes.IterateNext();

				// Checks if there are any more iterations to contend; if null, exit out
				if (chosenSudokuNodes == null)
					break;

				// Otherwise continue, and enter next loop
				do
				{
					OrderedSet<int> chosenSudokuNumbers = proposedSudokuNumbers.IterateNext();

					// Checks if there are any more iterations to contend; if null, exit out
					if (chosenSudokuNumbers == null)
						break;

					// Otherwise continue, and perform comparison
					bool passThrough = false;
					foreach (SNode sNode in chosenSudokuNodes)
					{
						// check if chosenNode can't be any of the chosenNumbers
						foreach (int value in chosenSudokuNumbers)
						{
							if (sNode.Possibles.Contains(value))
							{
								passThrough = true;
								break;
							}
						}

						if (passThrough)
							break;
					}

					if (passThrough)
						continue;

					bool boardUpdated = false;

					// It Passed...let's solve
					#region Build unchosenLists
					OrderedSet<SNode> unchosenSudokuNodes = new OrderedSet<SNode>();
					OrderedSet<int> unchosenSudokuNumbers = new OrderedSet<int>();
					foreach (SNode sNode in shrunkenSet)
					{
						if (!chosenSudokuNodes.Contains(sNode))
							unchosenSudokuNodes.Add(sNode);
					}
					foreach (int i in set.UnsolvedValues)
					{
						if (!chosenSudokuNumbers.Contains(i))
							unchosenSudokuNumbers.Add(i);
					}
					#endregion

					foreach (SNode sNode in unchosenSudokuNodes)
					{
						foreach (int value in unchosenSudokuNumbers)
						{
							if (sNode.Possibles.Contains(value))
							{
								sNode.Possibles.Remove(value);
								boardUpdated = true;
							}
						}
					}

					if (boardUpdated)
					{
						if (controller != null)
						{
							string numbers = "";
							string style = "";
							int adjustedSize = 0;
							if (size * 2 < shrunkenSet.Count)
							{
								// Naked Sets
								foreach (int number in unchosenSudokuNumbers)
									numbers += number + ", ";
								style = "Naked";
								adjustedSize = size;
							}
							else
							{
								// Hidden Sets
								foreach (int number in chosenSudokuNumbers)
									numbers += number + ", ";
								style = "Hidden";
								adjustedSize = shrunkenSetSize - size;
							}
							string n = numbers.Substring(0, numbers.Length - 2);
							string output = "Updated board with " + style + " " + adjustedSize + ".  " + set.GetType() + " " + set.Index + " Reduction:  Numbers " + n + " were used.";
							controller.PrintStatement(output);
						}

						return true;
					}

				} while (true);
			} while (true);
		}

		return false;
	} // Naked/HiddenSet()

	/// <summary>
	/// Loops through XWing solve for Rows->Columns and Columns->Rows.
	/// </summary>
	/// <returns></returns>
	private static bool XWingLoop()
	{
		if (XWing(board.Rows, board.Columns))
			return true;

		if (XWing(board.Columns, board.Rows))
			return true;

		return false;
	} // XWingLoop()

	/// <summary>
	/// Occurs when 2 rows share the same 2 columns, and not the other 7, that can be a certain number; all other spaces in those 2 columns CAN'T be this number now.
	/// </summary>
	/// <param name="primarySets"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	private static bool XWing(OrderedSet<SudokuSet> primarySets, OrderedSet<SudokuSet> secondarySets)
	{
		#region Lines
		CombinationNoRepetition<SudokuSet> proposedSetOfSudokuLines = new CombinationNoRepetition<SudokuSet>(primarySets, 2);
		#endregion

		do
		{
			OrderedSet<SudokuSet> chosenSudokuLines = proposedSetOfSudokuLines.IterateNext();

			if (chosenSudokuLines == null)
				return false;

			List<SudokuSet> listOfLines = new List<SudokuSet>();
			foreach (SudokuSet line in chosenSudokuLines)
				listOfLines.Add(line.Trim());

			for (int value = 1; value <= setSize; value++)
			{
				OrderedSet<SudokuSet> secondaryLines = new OrderedSet<SudokuSet>();

				List<SNode> listOfXWingSNodes = new List<SNode>();

				// Check each space in the first line
				foreach (SNode sNode in listOfLines[0])
				{
					if (sNode.Possibles.Contains(value))
					{
						foreach (SudokuSet secondaryLine in secondarySets)
						{
							if (secondaryLine.Contains(sNode))
							{
								secondaryLines.Add(secondaryLine);
								break;
							}
						}

						listOfXWingSNodes.Add(sNode);
					}
				}

				if (secondaryLines.Count != 2)
					continue;
				int count = 0;

				// Check each space in the second line
				foreach (SNode sNode in listOfLines[1])
				{
					if (sNode.Possibles.Contains(value))
					{
						foreach (SudokuSet secondaryLine in secondarySets)
						{
							if (secondaryLine.Contains(sNode))
							{
								if (!secondaryLines.Add(secondaryLine))
									count++;
								break;
							}
						}

						listOfXWingSNodes.Add(sNode);
					}
				}

				if (secondaryLines.Count != 2 || count != 2)
					continue;
				else
				{
					// Success!!!  Remove impossibles.
					bool boardUpdated = false;

					foreach (SudokuSet secondaryLine in secondaryLines)
					{
						foreach (SNode sNode in secondaryLine.Trim())
						{
							if (!listOfXWingSNodes.Contains(sNode) && sNode.Possibles.Contains(value))
							{
								sNode.Possibles.Remove(value);
								boardUpdated = true;
							}
						}
					}

					if (boardUpdated)
					{
						if (controller != null)
						{
							string output = "Updated board with XWing Reduction. " + value + " was used.";
							controller.PrintStatement(output);
						}

						return true;
					}
				}
			}
			// end of loop
		} while (true);
	} // XWing()
	#endregion

	/// <summary>
	/// Checks if the puzzle is unsolved.
	/// </summary>
	/// <returns></returns>
	private static bool IsSolved()
	{
		foreach (SNode sNode in board.Sudoku)
		{
			if (!sNode.IsSolved)
				return false;
		}

		return true;
	} // IsSolved()

	/// <summary>
	/// Prints the values of the board to console.
	/// </summary>
	private static void printBoardData()
	{
		foreach (SNode sNode in board.Sudoku)
		{
			if (controller != null)
				controller.PrintStatement(sNode.PrintPossibles());
		}
	} // printBoardData()
}