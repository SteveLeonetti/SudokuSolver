using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuBoard
{
    public SNode[,] Sudoku;
    public Dictionary<int, Dictionary<int, SNode>> Rows;
    public Dictionary<int, Dictionary<int, SNode>> Columns;
    public Dictionary<int, Dictionary<int, SNode>> Boxes;
    public Dictionary<string, Dictionary<int, SNode>> Sets;

    /// <summary>
    ///  Construct Sudoku Board data from the SNode objects by placing them in their relative sets (row, column, and box that they belong to)
    /// </summary>
    /// <param name="_sudoku"></param>
    public SudokuBoard(SNode[,] _sudoku)
    {
        Rows = new Dictionary<int, Dictionary<int, SNode>>();
        Columns = new Dictionary<int, Dictionary<int, SNode>>();
        Boxes = new Dictionary<int, Dictionary<int, SNode>>();
        Sets = new Dictionary<string, Dictionary<int, SNode>>();

        Sudoku = _sudoku;

        applySetRelationships();

        unifySets();
    }

    /// <summary>
    /// Creates rows, columns, and boxes in data to be used later.
    /// </summary>
    private void applySetRelationships()
    {
        #region Load Rows
        for (int row = 1; row <= 9; row++)
        {
            Rows.Add(row, new Dictionary<int, SNode>());
            for (int node = 1; node <= 9; node++)
            {
                //
                //
                SNode sNode = Sudoku[node - 1, row - 1];
                Rows[row].Add(node, sNode);
                Sudoku[node - 1, row - 1].Row = Rows[row];
                //
                //
            }
        }
        #endregion

        #region Load Columns
        for (int column = 1; column <= 9; column++)
        {
            Columns.Add(column, new Dictionary<int, SNode>());
            for (int node = 1; node <= 9; node++)
            {
                //
                //
                SNode sNode = Sudoku[column - 1, node - 1];
                Columns[column].Add(node, sNode);
                Sudoku[column - 1, node - 1].Column = Columns[column];
                //
                //
            }
        }
        #endregion

        #region Load Boxes
        int startRow = 0;
        int startColumn = 0;

        for (int box = 1; box <= 9; box++)
        {
            Boxes.Add(box, new Dictionary<int, SNode>());
        
            for (int down = 0; down < 3; down++)
            {
                for (int right = 0; right < 3; right++)
                {
                    //
                    //
                    SNode sNode = Sudoku[startColumn + right, startRow + down];
                    Boxes[box].Add(1 + (3 * down) + right, sNode);
                    Sudoku[startColumn + right, startRow + down].Box = Boxes[box];
                    //
                    //
                }
            }

            // Sets up the starter indices for each box
            if (startColumn == 6)
            {
                startColumn = 0;
                startRow += 3;
            }
            else
                startColumn += 3;
        }
        #endregion
    }

    /// <summary>
    /// Places all rows, columns, and boxes data into a single dictionary called "sets"
    /// </summary>
    private void unifySets()
    {
        #region Union of all [row, column, box] sets into Dictionary called 'sets'
        foreach (KeyValuePair<int, Dictionary<int, SNode>> kvp in Rows)
        {
            Sets.Add("r" + kvp.Key, kvp.Value);
        }
        foreach (KeyValuePair<int, Dictionary<int, SNode>> kvp in Columns)
        {
            Sets.Add("c" + kvp.Key, kvp.Value);
        }
        foreach (KeyValuePair<int, Dictionary<int, SNode>> kvp in Boxes)
        {
            Sets.Add("b" + kvp.Key, kvp.Value);
        }
        #endregion
    }
}