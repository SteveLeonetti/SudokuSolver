
using System.Collections.Generic;
/// <summary>
/// 1. Check single boxes if they can only be 1 value. (Repeat until no progress)
/// 2. Check sets (row, column, box) if a single box in that set can be a certain value. (Regress to Stage 1 if successful)
/// 3. Check for restrictive sets within a set (row, column, box). (Regress to Stage 1 if successful)
/// </summary>
public static class Solver
{
    static int width;
    static int height;
    static SNode[,] sudoku;

    /// <summary>
    /// Solves the puzzle as best it can
    /// </summary>
    /// <param name="s"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <returns>integer type board</returns>
    public static int[,] Solve(SNode[,] s, int w, int h)
    {
        int maxLoopCount = 500;
        int curCount = 0;

        Load(s, w, h); // Loads initial state with predetermined easy "not-possibles"

        while (!IsSolved())
        {
            if (curCount >= maxLoopCount)
                break;
            curCount++;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    //
                    //
                    if (sudoku[x, y].possRem == 1)
                        OnlyPossibleOnNode(x, y);
                    //
                    //
                }
            }

            for (int i = 0; i < h; i++)
            {
                OnlyPossibleInSet(sudoku[i, i].row);
                OnlyPossibleInSet(sudoku[i, i].column);
            }

            OnlyPossibleInSet(sudoku[0, 0].box);
            OnlyPossibleInSet(sudoku[0, 3].box);
            OnlyPossibleInSet(sudoku[0, 6].box);

            OnlyPossibleInSet(sudoku[3, 0].box);
            OnlyPossibleInSet(sudoku[3, 3].box);
            OnlyPossibleInSet(sudoku[3, 6].box);

            OnlyPossibleInSet(sudoku[6, 0].box);
            OnlyPossibleInSet(sudoku[6, 3].box);
            OnlyPossibleInSet(sudoku[6, 6].box);

            if (!CheckLockedSets())
            {

            }
            // Add more logic later
            /* Next idea: save SNode[*,*] states, and make guesses if neccesary */




            //
            //



        }

        return SNode.ConvertToOutput(sudoku, width, height);
    }

    /// <summary>
    /// Loads data about sudoku board.
    /// </summary>
    /// <param name="s">Sudoku Board</param>
    /// <param name="w">Width</param>
    /// <param name="h">Height</param>
    private static void Load(SNode[,] s, int w, int h)
    {
        sudoku = s;
        width = w;
        height = h;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                //
                //
                sudoku[x, y].SetRow(LoadRow(x, y));
                sudoku[x, y].SetColumn(LoadColumn(x, y));
                sudoku[x, y].SetBox(LoadBox(x, y));
                CheckImpossibles(x, y);
                //
                //
            }
        }
    } // Load()

    #region LoadSets

    /// <summary>
    /// Creates a Node[] of each node contained in the Row that contains the node of interest in it.
    /// </summary>
    /// <param name="y">Lock</param>
    /// <returns>Row</returns>
    private static SNode[] LoadRow(int x, int y)
    {
        SNode[] row = new SNode[width];
        for (int i = 0; i < width; i++)
            row[i] = sudoku[i, y];

        return row;
    }

    /// <summary>
    /// Creates a Node[] of each node contained in the Column that contains the node of interest in it.
    /// </summary>
    /// <param name="x">Lock</param>
    /// <returns>Column</returns>
    private static SNode[] LoadColumn(int x, int y)
    {
        SNode[] column = new SNode[height];
        for (int i = 0; i < height; i++)
            column[i] = sudoku[x, i];

        return column;
    }

    /// <summary>
    /// Creates a Node[] of each node contained in the Box that contains the node of interest in it.
    /// </summary>
    /// <param name="x">variable</param>
    /// <param name="y">variable</param>
    /// <returns>Box</returns>
    private static SNode[] LoadBox(int x, int y)
    {
        SNode[] box = new SNode[(width / 3) * (height / 3)];

        int boxX = x / 3;
        int boxY = y / 3;
        int i = 0;


        for (int yTar = boxY * 3; yTar < (boxY + 1) * 3; yTar++)
        {
            for (int xTar = boxX * 3; xTar < (boxX + 1) * 3; xTar++)
            {
                //
                //
                box[i++] = sudoku[xTar, yTar];
                //
                //
            }
        }


        return box;
    }

    #endregion
    // done

    #region Check

    /// <summary>
    /// Checks nodes versus it's containing Row, Column, and Box to see what numbers it CAN'T be.
    /// Assigns false boolean to spaces in the bool[] named possibles for said node.
    /// </summary>
    private static void CheckImpossibles(int x, int y)
    {
        if (!sudoku[x, y].isSolved)
        {
            foreach (SNode rn in sudoku[x, y].row)
            {
                if (rn.isSolved)
                {
                    if (rn.value > 0)
                        sudoku[x, y].possibles[rn.value - 1] = false;
                }
            }
            foreach (SNode cn in sudoku[x, y].column)
            {
                if (cn.isSolved)
                {
                    if (cn.value > 0)
                        sudoku[x, y].possibles[cn.value - 1] = false;
                }
                    
            }
            foreach (SNode bn in sudoku[x, y].box)
            {
                if (bn.isSolved)
                {
                    if (bn.value > 0)
                        sudoku[x, y].possibles[bn.value - 1] = false;
                }
            }
        }
    } // CheckImpossibles()

    /// <summary>
    /// Checks if any set of n nodes, where the only values possible are those n number of values.
    /// </summary>
    private static bool CheckLockedSets()
    {
        bool changeOccurred = false;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //
                //
                for (int i = 2; i < 4; i++)
                {
                    if (!changeOccurred)
                        changeOccurred = CheckSet(sudoku[x, y].row, i);
                    else
                        CheckSet(sudoku[x, y].row, i);

                    if (!changeOccurred)
                        changeOccurred = CheckSet(sudoku[x, y].column, i);
                    else
                        CheckSet(sudoku[x, y].column, i);

                    if (!changeOccurred)
                        changeOccurred = CheckSet(sudoku[x, y].box, i);
                    else
                        CheckSet(sudoku[x, y].box, i);
                }
                //
                //
            }
        }

        return changeOccurred;
    } // CheckLockedSets()

    /// <summary>
    /// Checks to see if 'count' number of nodes are the only nodes in the set that can be 'count' number of certain values.
    /// If so, remove all possiblities of all other values.
    /// Example: If Nodes (1, 4, 5) are the only nodes [in a row, column, or box] that can store the Values (3, 6, 8), then Node (1), which has a Possibility (1, 2, 3, 6, 8, 9),
    /// is changed to Possibility (3, 6, 8), thus removing a up to a few possibilities off each of a few nodes, like 1, 2, and 9 were removed from this node.
    /// </summary>
    /// <param name="set"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private static bool CheckSet(SNode[] set, int count)
    {
        bool changeOccurred = false;

        int[] individualPossibilitiesCount = new int[9];
        int indexStep = -1;

        // Iterate through each number, if any count numbers are only able to be placed on the same number of nodes, remove all other possibles from those select nodes.
        for (int valIndex = 0; valIndex < set.Length; valIndex++)
        {
            // Check for number of nodes that can be this value.  If equal to count, save it.
            foreach (SNode sn in set)
            {
                if (!sn.isSolved && sn.possibles[valIndex])
                    individualPossibilitiesCount[valIndex]++;
            }
        }

        // Find out which indices in individualPossibilitiesCount that are equal to count (we don't know which SNodes they are yet)
        int[] goodPossibilitiesIndexes = new int[9];

        for (int valIndex = 0; valIndex < 9; valIndex++)
        {
            if (individualPossibilitiesCount[valIndex] != count)
                individualPossibilitiesCount[valIndex] = 0;
            else
                goodPossibilitiesIndexes[++indexStep] = valIndex;  // 3, 4, 6 (for the values 4, 5, 7 respectively)
        }

        //goodPossibilitiesIndexes = condenseArray(goodPossibilitiesIndexes, indexStep);

        if (indexStep >= count)     // If the number of values is less than the expected number of matches, just fail and skip.
        {
            Dictionary<int, List<SNode>> snodeLists = new Dictionary<int, List<SNode>>();
            for (int i = 0; i < indexStep; i++)
                snodeLists.Add(goodPossibilitiesIndexes[i], new List<SNode>());     // Should store {3 -> SNodes that can be 4 (index 3), 4 -> SNodes that can be 5(index 4), ... and so on}

            for (int i = 0; i < goodPossibilitiesIndexes.Length; i++)
            {
                try
                {
                    for (int x = 0; x < 9; x++)     // Goes through all 9 SNodes in set
                    if (!set[x].isSolved && set[x].possibles[goodPossibilitiesIndexes[i]])
                        snodeLists[goodPossibilitiesIndexes[i]].Add(set[x]);
                }
#pragma warning disable CS0168 // Variable is declared but never used
                catch (KeyNotFoundException knfe)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    break;
                }
                
            }
            
            int[] heldIndices = new int[count];     // Should store {0, 1}, then {0, 2}, then {1, 2}, as a 3 choose 2 iteration, etc for 4 choose 2, 4 choose 3, etc again
            int numPossibles = indexStep;
            int currIndex = count - 1;
            bool exitLoop = false;

            for (int i = 0; i < count; i++)
                heldIndices[i] = i;

            do
            {
                // Check current heldIndices
                if (heldIndicesAreSame(heldIndices, goodPossibilitiesIndexes, snodeLists))
                {
                    // SUCCESS, we can remove all non-believers!!!
                    // (all possibles from these indexes that don't match the pairs of goodPossibles expected)
                    
                    SNode[] valuableList = snodeLists[goodPossibilitiesIndexes[heldIndices[0]]].ToArray();

                    bool[] truthMask = new bool[9];

                    for (int i = 0; i < count; i++)
                        truthMask[goodPossibilitiesIndexes[heldIndices[i]]] = true;

                    foreach (SNode sn in valuableList)
                    {
                        // for each node, truthMask will replace the original possibilities array
                        for (int i = 0; i < 9; i++)
                        {
                            if (!truthMask[i] && sn.possibles[i])
                                changeOccurred = true;

                            sn.possibles[i] = truthMask[i];
                        }
                    }

                    break;
                }

                while (heldIndices[currIndex] == numPossibles - 1 || (currIndex < count - 1 && heldIndices[currIndex] + 1 == heldIndices[currIndex + 1]))
                {
                    // This means that the current index is as far right as it can go.
                    currIndex--;
                    if (currIndex < 0)
                        break;
                }

                if (currIndex < 0)  // List is iterated all the way through
                    break;

                heldIndices[currIndex]++;

                // Update all trailing indices to follow currIndex
                for (int i = currIndex + 1; i < numPossibles - 1; i++)
                    heldIndices[i] = heldIndices[i - 1] + 1;

                currIndex = count - 1;

            } while (!exitLoop);
            
        }

        return changeOccurred;
    }

    /// <summary>
    /// Takes the list of goodPossibles, and ONLY using the subset of that required for count, check to see a single snap shot of all required iterations to check if are same.
    /// </summary>
    /// <param name="heldIndices"></param>
    /// <param name="goodPossibleIndexes"></param>
    /// <param name="snodeLists"></param>
    /// <returns></returns>
    private static bool heldIndicesAreSame(int[] heldIndices, int[] goodPossibleIndexes, Dictionary<int, List<SNode>> snodeLists)
    {
        List<SNode> comparingList = snodeLists[goodPossibleIndexes[heldIndices[0]]];

        for (int i = 1; i < heldIndices.Length; i++)
        {
            if (!twoArraysAreSame(snodeLists[goodPossibleIndexes[heldIndices[i]]].ToArray(), comparingList.ToArray()))
                return false;
        }
        
        return true;
    } // heldIndicesAreSame()

    /// <summary>
    /// Checks to see if each array contains the same SNodes in the list. Each, in theory, should always have the same count.
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns></returns>
    private static bool twoArraysAreSame(SNode[] list1, SNode[] list2)
    {
        for (int i = 0; i < list1.Length; i++)
        {
            if (!list1[i].Equals(list2[i]))
                return false;
        }

        return true;
    } //twoArraysAreSame()

    #endregion

    #region Attempt

    /// <summary>
    /// If a single node can ONLY be one number, assign that number to the int named value, and null out the bool[] named possibles.
    /// </summary>
    private static bool OnlyPossibleOnNode(int x, int y)
    {
        bool changeOccurred = false;
        int count = 0;
        int spot = 0;

        if (!sudoku[x, y].isSolved)
        {
            for (int i = 0; i < ((width / 3) * (height / 3)); i++)
            {
                if (sudoku[x, y].possibles[i])
                {
                    count++;
                    spot = i;
                }
            }

            if (count == 1)
            {
                ApplyValue(sudoku[x, y], spot + 1);
                changeOccurred = true;
            }
        }

        return changeOccurred;
    } // OnlyPossibleOnNode()

    /// <summary>
    /// If a single node ON a single line or box is the ONLY node that can be some number, assign that number to the int named value, and null out the bool[] named possibles.
    /// </summary>
    private static bool OnlyPossibleInSet(SNode[] set)
    {
        bool changeOccurred = false;
        int count = 0;
        int spot = 0;
        SNode onlyNode = null;

        // Try each Value
        for (int vIndex = 0; vIndex < ((width / 3) * (height / 3)); vIndex++)
        {
            int value = vIndex + 1;

            count = 0;

            foreach (SNode sn in set)
            {
                if (!sn.isSolved && sn.possibles[vIndex])
                {
                    count++;
                    spot = vIndex;
                    onlyNode = sn;
                }
            }

            if (count == 1 && onlyNode.possibles[vIndex])
            {
                ApplyValue(onlyNode, value);
                changeOccurred = true;
            }
        }

        return changeOccurred;
    } // OnlyPossibleInSet()

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
        foreach (SNode s in n.row)
        {
            if (s != n && !s.isSolved && s.possibles[valIndex])
            {
                s.possibles[valIndex] = false;
                s.possRem--;
            }
        }

        foreach (SNode s in n.column)
        {
            if (s != n && !s.isSolved && s.possibles[valIndex])
            {
                s.possibles[valIndex] = false;
                s.possRem--;
            }
        }

        foreach (SNode s in n.box)
        {
            if (s != n && !s.isSolved && s.possibles[valIndex])
            {
                s.possibles[valIndex] = false;
                s.possRem--;
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
        for (int yIndex = 0; yIndex < height; yIndex++)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                if (!sudoku[xIndex, yIndex].isSolved)
                    return false;
            }
        }

        return true;
    } // IsSolved()

    // FORGET PURPOSE OF THIS METHOD BELOW...
    private static int[] condenseArray(int[] a)
    {

        return null;
    }

    private static void printBoardData()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                //
                //
                sudoku[x, y].PrintPossibles();
                //
                //
            }
            // Can't use print(), since this is not a MonoBehaviour
        }
    }
}