using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuBoard
{
    public SNode[,] Sudoku;

    public OrderedSet<SudokuSet> Rows;
    public OrderedSet<SudokuSet> Columns;
    public OrderedSet<SudokuSet> Boxes;
    public OrderedSet<SudokuSet> Sets;

    /// <summary>
    ///  Construct Sudoku Board data from the SNode objects by placing them in their relative sets (row, column, and box that they belong to)
    /// </summary>
    /// <param name="_sudoku"></param>
    public SudokuBoard(SNode[,] _sudoku)
    {
        Rows = new OrderedSet<SudokuSet>();
        Columns = new OrderedSet<SudokuSet>();
        Boxes = new OrderedSet<SudokuSet>();
        Sets = new OrderedSet<SudokuSet>();

        Sudoku = _sudoku;

        applySetRelationships();

        unifySets();
    }

    public SudokuBoard(string csv)
    {
        Rows = new OrderedSet<SudokuSet>();
        Columns = new OrderedSet<SudokuSet>();
        Boxes = new OrderedSet<SudokuSet>();
        Sets = new OrderedSet<SudokuSet>();

        Sudoku = ConvertFromCSV(csv);

        applySetRelationships();

        unifySets();
    }

    /// <summary>
    /// Creates rows, columns, and boxes in data to be used later.
    /// </summary>
    private void applySetRelationships()
    {
        #region Load Rows
        for (int index = 1; index <= 9; index++)
        {
            Row currentRow = new Row(index);
            currentRow.UnsolvedValues.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            for (int node = 1; node <= 9; node++)
            {
                //
                //
                SNode sNode = Sudoku[node - 1, index - 1];
                currentRow.Add(sNode);
                sNode.Row = currentRow;

                if (sNode.Value != 0)
                    currentRow.ValueSolved(sNode.Value);
                //
                //
            }

            Rows.Add(currentRow);
        }
        #endregion

        #region Load Columns
        for (int index = 1; index <= 9; index++)
        {
            Column currentColumn = new Column(index);
            currentColumn.UnsolvedValues.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            for (int node = 1; node <= 9; node++)
            {
                //
                //
                SNode sNode = Sudoku[index - 1, node - 1];
                currentColumn.Add(sNode);
                sNode.Column = currentColumn;

                if (sNode.Value != 0)
                    currentColumn.ValueSolved(sNode.Value);
                //
                //
            }

            Columns.Add(currentColumn);
        }
        #endregion

        #region Load Boxes
        int startRow = 0;
        int startColumn = 0;

        for (int index = 1; index <= 9; index++)
        {
            Box currentBox = new Box(index);
            currentBox.UnsolvedValues.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            for (int down = 0; down < 3; down++)
            {
                for (int right = 0; right < 3; right++)
                {
                    //
                    //
                    SNode sNode = Sudoku[startColumn + right, startRow + down];
                    currentBox.Add(sNode);
                    sNode.Box = currentBox;

                    if (sNode.Value != 0)
                        currentBox.ValueSolved(sNode.Value);
                    //
                    //
                }
            }

            Boxes.Add(currentBox);

            currentBox.AddColumn(currentBox[0].Column);
            currentBox.AddRow(currentBox[0].Row);
            currentBox.AddColumn(currentBox[4].Column);
            currentBox.AddRow(currentBox[4].Row);
            currentBox.AddColumn(currentBox[8].Column);
            currentBox.AddRow(currentBox[8].Row);

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
        #region Union of all [row, column, box] sets into OrderedSet called 'sets'
        foreach (Row row in Rows)
            Sets.Add(row);

        foreach (Column column in Columns)
            Sets.Add(column);

        foreach (Box box in Boxes)
            Sets.Add(box);
        #endregion
    }

    private SNode[,] ConvertFromCSV(string csv)
    {
        SNode[,] conversion = new SNode[9, 9];
        OrderedSet<int> possibles = new OrderedSet<int>();

        string sValue;
        string sPossibles;
        string currentNode;

        for (int c = 0; c < 9; c++)
        {
            for(int r = 0;  r < 9; r++)
            {
                if (csv.IndexOf(",") == 0)
                    csv = csv.Substring(1);
                currentNode = csv.Substring(0, csv.IndexOf(','));
                sValue = currentNode.Substring(0, currentNode.IndexOf(':'));
                int value = int.Parse(sValue);

                sPossibles = currentNode.Substring(currentNode.IndexOf(':') + 1);
                foreach (char ch in sPossibles)
                    possibles.Add(int.Parse(ch.ToString()));

                if (value != 0)
                    conversion[c, r] = new SNode(value);
                else
                    conversion[c, r] = new SNode(possibles);

                csv = csv.Remove(0, csv.IndexOf(','));
            }
        }

        return conversion;
    }

    public override string ToString()
    {
        string csv = "";
        for (int c = 0; c < 9; c++)
        {
            for (int r = 0; r < 9; r++)
            {
                csv += Sudoku[c, r].Value.ToString();
                string possibles = ":";
                foreach (int i in Sudoku[c, r].Possibles)
                    possibles += i.ToString();

                csv += possibles;
                csv += ",";
            }
        }

        return csv;
    }
}