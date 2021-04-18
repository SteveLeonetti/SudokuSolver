using System;
using System.Collections.Generic;

/// <summary>
/// 1. Check single boxes if they can only be 1 value. (Repeat until no progress)
/// 2. Check sets (row, column, box) if a single box in that set can be a certain value. (Regress to Stage 1 if successful)
/// 3. Check for restrictive sets within a set (row, column, box). (Regress to Stage 1 if successful)
/// </summary>
public static class Solver
{
    static int setSize;
    static SudokuBoard board;
    static OrderedSet<int> sudokuNumbers = new OrderedSet<int>();
    static Controller controller;

    /// <summary>
    /// Solves the puzzle as best it can.  This is the method portraying the logical order of solving.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="c"></param>
    /// <returns>integer type board</returns>
    public static int[,] Solve(SNode[,] s, Controller c)
    {
        controller = c;

        Load(s); // Loads initial state with predetermined obvious "not-possibles"

        while (!IsSolved())
        {
            // Precedence 1: Remove some possibles with this solve technique. Pointing Pairs.
            if (PointingPair())
                continue;

            // Precedence 2: Remove some possibles with this solve technique.  Line/Box Reduction.
            if (LineBoxReduction())
                continue;

            // Precedence 3: Remove some possibles with this solve technique.  Naked, Single, Pair, Triplet, Quadruplet, etc...
            if (SetLoop())
                continue;

            // Precedence 4: Remove some possibles with this solve technique.  XWing[Row->Col] or XWing[Col->Row]
            if (XWingLoop())
                continue;


            // Add more logic later
            /* Next idea: save SNode[*,*] states, and make guessing tree if neccesary */




            //
            //



            break;
        }

        if (controller != null)
        {
            string output = "Solver has done as much as it could do!  ";
            controller.PrintStatement(output);
        }

        return SNode.ConvertToOutput(board);
    } // Solve()

    /// <summary>
    /// Loads data about sudoku board.
    /// </summary>
    /// <param name="s">Sudoku Board</param>
    private static void Load(SNode[,] s)
    {
        setSize = s.GetLength(0);
        
        for (int i = 1; i <= setSize; i++)
            sudokuNumbers.Add(i);

        board = new SudokuBoard(s);

        foreach (SNode sNode in board.Sudoku)
        {
            CheckImpossibles(sNode);
        }
    } // Load()

    #region Check
    /// <summary>
    /// Checks nodes versus it's containing Row, Column, and Box to see what numbers it CAN'T be.
    /// Assigns false boolean to spaces in the bool[] named possibles for said node.
    /// </summary>
    private static void CheckImpossibles(SNode sNode)
    {
        if (!sNode.isSolved)
        {
            #region Row
            foreach (SNode rowNode in sNode.Row.Values)
            {
                if (rowNode.isSolved)
                {
                    if (rowNode.value > 0)
                    {
                        if (!sNode.isSolved && sNode.possibles[rowNode.value - 1])
                        {
                            sNode.possibles[rowNode.value - 1] = false;
                            sNode.possRem--;

                            if (sNode.possRem == 1)
                                NakedSingle(sNode);
                        }
                    }
                }
            }
            #endregion

            #region Column
            foreach (SNode columnNode in sNode.Column.Values)
            {
                if (columnNode.isSolved)
                {
                    if (columnNode.value > 0)
                    {
                        if (!sNode.isSolved && sNode.possibles[columnNode.value - 1])
                        {
                            sNode.possibles[columnNode.value - 1] = false;
                            sNode.possRem--;

                            if (sNode.possRem == 1)
                                NakedSingle(sNode);
                        }
                    }
                }
            }
            #endregion

            #region Box
            foreach (SNode boxNode in sNode.Box.Values)
            {
                if (boxNode.isSolved)
                {
                    if (boxNode.value > 0)
                    {
                        if (!sNode.isSolved && sNode.possibles[boxNode.value - 1])
                        {
                            sNode.possibles[boxNode.value - 1] = false;
                            sNode.possRem--;

                            if (sNode.possRem == 1)
                                NakedSingle(sNode);
                        }
                    }
                }
            }
            #endregion
        }
    } // CheckImpossibles()

    /// <summary>
    /// If a single node can ONLY be one number, assign that number to the int named value, and null out the bool[] named possibles.
    /// </summary>
    private static bool NakedSingle(SNode sNode)
    {
        if (!sNode.isSolved)
        {
            int i = Array.IndexOf(sNode.possibles, true);

            ApplyValue(sNode, i + 1);

            return true;
        }

        return false;
    } // NakedSingle()

    /// <summary>
    /// Finds Pointing Pairs.
    /// </summary>
    /// <returns></returns>
    private static bool PointingPair()
    {
        foreach (Dictionary<int,SNode> box in board.Boxes.Values)
        {
            foreach (int value in sudokuNumbers)
            {
                OrderedSet<Dictionary<int, SNode>> rows = new OrderedSet<Dictionary<int, SNode>>();
                OrderedSet<Dictionary<int, SNode>> cols = new OrderedSet<Dictionary<int, SNode>>();

                foreach (SNode sNode in box.Values)
                {
                    if (!sNode.isSolved && sNode.possibles[value - 1])
                    {
                        rows.Add(sNode.Row);
                        cols.Add(sNode.Column);
                    }
                }

                bool boardUpdated = false;

                if (rows.Count == 1)
                {
                    foreach (SNode sNode in rows[0].Values)
                    {
                        if (!sNode.isSolved && sNode.Box != box && sNode.possibles[value - 1])
                        {
                            sNode.possibles[value - 1] = false;
                            sNode.possRem--;
                            boardUpdated = true;

                            if (sNode.possRem == 1)
                                NakedSingle(sNode);
                        }
                    }
                }

                if (cols.Count == 1)
                {
                    foreach (SNode sNode in cols[0].Values)
                    {
                        if (!sNode.isSolved && sNode.Box != box && sNode.possibles[value - 1])
                        {
                            sNode.possibles[value - 1] = false;
                            sNode.possRem--;
                            boardUpdated = true;

                            if (sNode.possRem == 1)
                                NakedSingle(sNode);
                        }
                    }
                }

                if (boardUpdated)
                {
                    if (controller != null)
                    {
                        string output = "Updated board with Pointing Pair Solve. " + value + " was used.";
                        controller.PrintStatement(output);
                    }

                    return true;
                }
            }
        }

        return false;
    } // PointingPair()

    /// <summary>
    /// Finds Line/Box Reductions.
    /// </summary>
    /// <returns></returns>
    private static bool LineBoxReduction()
    {

        return false;
    } // LineBoxReduction()

    /// <summary>
    /// Looping structure for solving Naked Sets.  Uses CombinationNoRepitition to check all chances.
    /// </summary>
    /// <returns></returns>
    private static bool SetLoop()
    {
        bool progress = false;
        int[] easinessRating = { 1, 8, 2, 7, 3, 6, 4, 5 };
        /*
            Variable of easinessRating depends on how long the loops may need to run
            1. Easiest:         1 and 8                     (~100 loops)
            2. Moderate:    2 and 7                     (~1300 loops)
            3. Hard:            3 and 6                     (~7000 loops)
            4. Hardest:       4 and 5                     (~16000 loops)
        */

        for (int place = 0; place < easinessRating.Length; place++)
        {
            progress = NakedHiddenSet(easinessRating[place]);

            if (progress)
                break;
        }

        return progress;
    } // SetLoop()

    /// <summary>
    /// Using the size provided from the parameter, finds Naked sets of that size.
    /// IE: Size 1 is a Naked Single, Size 3 is a Naked Triplet, and Size 8 is a Naked Octet (AKA Hidden Single)
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private static bool NakedHiddenSet(int size)
    {
        foreach (KeyValuePair<string, Dictionary<int, SNode>> kvpSet in board.Sets)
        {
            string setName = kvpSet.Key;
            Dictionary<int, SNode> set = kvpSet.Value;

            #region Nodes in set saved in OrderedSet variable : orderedNodes
            OrderedSet<SNode> orderedNodes = new OrderedSet<SNode>();
            foreach (SNode node in set.Values)
                orderedNodes.Add(node);
            #endregion

            #region Nodes
            CombinationNoRepetition<SNode> proposedSetOfSudokuNodes = new CombinationNoRepetition<SNode>(orderedNodes, size);
            #endregion

            #region Numbers
            CombinationNoRepetition<int> proposedSudokuNumbers = new CombinationNoRepetition<int>(sudokuNumbers, set.Count - size);
            #endregion

            // the "IterateNext()" code will need to be inside of a continuable nested loop
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
                        if (sNode.isSolved || sNode.possibles == null)
                        {
                            passThrough = true;
                            break;
                        }

                        // check if chosenNode can't be any of the chosenNumbers
                        foreach (int i in chosenSudokuNumbers)
                        {
                            if (sNode.possibles[i - 1])
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
                    foreach (SNode sNode in set.Values)
                    {
                        if (!chosenSudokuNodes.Contains(sNode))
                            unchosenSudokuNodes.Add(sNode);
                    }
                    foreach (int i in sudokuNumbers)
                    {
                        if (!chosenSudokuNumbers.Contains(i))
                            unchosenSudokuNumbers.Add(i);
                    }
                    #endregion

                    foreach (SNode sNode in unchosenSudokuNodes)
                    {
                        if (sNode.isSolved) // If the SNode is solved, the possibles array is null and doesn't need updating even if it wasn't null.
                            continue;

                        foreach (int number in unchosenSudokuNumbers)
                        {
                            if (sNode.possibles != null && sNode.possibles[number - 1])
                            {
                                sNode.possibles[number - 1] = false;
                                sNode.possRem--;
                                boardUpdated = true;

                                if (sNode.possRem == 1)
                                    NakedSingle(sNode);
                            }
                        }
                    }

                    if (boardUpdated)
                    {
                        if (controller != null)
                        {
                            string numbers = "";
                            foreach (int number in unchosenSudokuNumbers)
                                numbers += number + ", ";
                            string n = numbers.Substring(0, numbers.Length - 2);
                            string output = "Updated board with " + size + " Naked/Hidden.  "  + setName + ":  Numbers " + n + " were used.";
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
        else
            return false;
    } // XWingLoop()

    /// <summary>
    /// Check if this works....I've, as I think, completed it.
    /// </summary>
    /// <param name="primarySets"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool XWing(Dictionary<int, Dictionary<int, SNode>> primarySets, Dictionary<int, Dictionary<int, SNode>> secondarySets)
    {
        #region Lines in sets saved in OrderedSet variable : orderedLines
        OrderedSet<Dictionary<int, SNode>> orderedLines = new OrderedSet<Dictionary<int, SNode>>();
        foreach (Dictionary<int, SNode> line in primarySets.Values)
            orderedLines.Add(line);
        #endregion

        #region Lines
        CombinationNoRepetition<Dictionary<int, SNode>> proposedSetOfSudokuLines = new CombinationNoRepetition<Dictionary<int, SNode>>(orderedLines, 2);
        #endregion

        do
        {
            OrderedSet<Dictionary<int, SNode>> chosenSudokuLines = proposedSetOfSudokuLines.IterateNext();

            if (chosenSudokuLines == null)
                return false;

            List<Dictionary<int, SNode>> listOfLines = new List<Dictionary<int, SNode>>();
            foreach (Dictionary<int, SNode> line in chosenSudokuLines)
                listOfLines.Add(line);

            for (int value = 1; value <= setSize; value++)
            {
                OrderedSet<Dictionary<int, SNode>> secondaryLines = new OrderedSet<Dictionary<int, SNode>>();

                List<SNode> listOfXWingSNodes = new List<SNode>();
                foreach (SNode sNode in listOfLines[0].Values)
                {
                    if (!sNode.isSolved && sNode.possibles[value - 1])
                    {
                        foreach (Dictionary<int, SNode> secondaryLine in secondarySets.Values)
                        {
                            if (secondaryLine.ContainsValue(sNode))
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

                foreach (SNode sNode in listOfLines[1].Values)
                {
                    if (!sNode.isSolved && sNode.possibles[value - 1])
                    {
                        foreach (Dictionary<int, SNode> secondaryLine in secondarySets.Values)
                        {
                            if (secondaryLine.ContainsValue(sNode))
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

                    foreach (Dictionary<int, SNode> secondaryLine in secondaryLines)
                    {
                        foreach (SNode sNode in secondaryLine.Values)
                        {
                            if (!listOfXWingSNodes.Contains(sNode) && sNode.possibles != null && sNode.possibles[value - 1])
                            {
                                sNode.possibles[value - 1] = false;
                                sNode.possRem--;
                                boardUpdated = true;

                                if (sNode.possRem == 1)
                                    NakedSingle(sNode);
                            }
                        }
                    }

                    if (boardUpdated)
                    {
                        if (controller != null)
                        {
                            string output = "Updated board with XWing Solve. " + value + " was used.";
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

    #region Application of values
    /// <summary>
    /// Applies the value to the node when it is the only possible result.
    /// </summary>
    /// <param name="n">node</param>
    /// <param name="v">value</param>
    private static void ApplyValue(SNode n, int v)
    {
        // Set value on SNode
        n.value = v;
        n.possibles = null;
        n.isSolved = true;


        // Adjust all nodes in row, column, and box [based on this change].
        int valIndex = v - 1;
        foreach (SNode s in n.Row.Values)
        {
            // Checking if it's also already false, if so, we don't enter this if
            if (s != n && !s.isSolved && s.possibles[valIndex])
            {
                s.possibles[valIndex] = false;
                s.possRem--;

                if (s.possRem == 1)
                    NakedSingle(s);
            }
        }

        foreach (SNode s in n.Column.Values)
        {
            // Checking if it's also already false, if so, we don't enter this if
            if (s != n && !s.isSolved && s.possibles[valIndex])
            {
                s.possibles[valIndex] = false;
                s.possRem--;

                if (s.possRem == 1)
                    NakedSingle(s);
            }
        }

        foreach (SNode s in n.Box.Values)
        {
            // Checking if it's also already false, if so, we don't enter this if
            if (s != n && !s.isSolved && s.possibles[valIndex])
            {
                s.possibles[valIndex] = false;
                s.possRem--;

                if (s.possRem == 1)
                    NakedSingle(s);
            }
        }
    } // ApplyValue()
    #endregion

    /// <summary>
    /// Checks if the puzzle is unsolved.
    /// </summary>
    /// <returns></returns>
    private static bool IsSolved()
    {
        foreach (SNode sNode in board.Sudoku)
        {
                if (!sNode.isSolved)
                    return false;
        }

        return true;
    } // IsSolved()

    // I think it was for reducing the iterations on combinations.  In cases were sets were going to include numbers that have been previously finalized
    // Like if a row had 4 solidly in a single block as the true result, and we used 4 and/or that block in the iterations (it shouldn't be needed)
    // AKA .trim() -- This method should exist on the Set class.  Set class should create and operate each box, row, and column.
    private static int[] condenseArray(int[] a)
    {

        return null;
    }

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
    }
}