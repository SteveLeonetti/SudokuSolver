using System;
using System.Collections.Generic;

public class SNode
{
    public int rowIndex = 0;
    public int columnIndex = 0;

    public int Value = 0;
    public OrderedSet<int> Possibles;
    public bool IsSolved;

    public Row Row { get; set; }

    public Column Column { get; set; }

    public Box Box { get; set; }

    #region Constructors
    public SNode(OrderedSet<int> _sudokuNumbers)
    {
        Value = 0;
        Possibles = new OrderedSet<int>();
        foreach (int value in _sudokuNumbers)
            Possibles.Add(value);
        IsSolved = false;
    }

    public SNode(int v)
    {
        Value = v;
        Possibles = new OrderedSet<int>();
        IsSolved = true;
    }
    #endregion

    #region Application of values
    /// <summary>
    /// Applies the value to the node when it is the only possible result.
    /// </summary>
    /// <param name="n">node</param>
    /// <param name="v">value</param>
    public bool ApplyValue(int v = 0)
    {
        // Set value on SNode
        if (v == 0 && Possibles.Count == 1)
            v = Possibles[0];

        Possibles.Clear();
        Value = v;
        IsSolved = true;
        Row.ValueSolved(v);
        Column.ValueSolved(v);
        Box.ValueSolved(v);

        if (v < 1 || v > 9)
            return false;

        // Adjust all nodes in row, column, and box [based on this change].
        foreach (SNode sNode in Row.Trim())
        {
            // Checking if it's also already false, if so, we don't enter this if
            if (sNode != this && sNode.Possibles.Contains(Value))
                sNode.Possibles.Remove(v);
        }

        foreach (SNode sNode in Column.Trim())
        {
            // Checking if it's also already false, if so, we don't enter this if
            if (sNode != this && sNode.Possibles.Contains(Value))
                sNode.Possibles.Remove(v);
        }

        foreach (SNode sNode in Box.Trim())
        {
            // Checking if it's also already false, if so, we don't enter this if
            if (sNode != this && sNode.Possibles.Contains(Value))
                sNode.Possibles.Remove(v);
        }

        return true;
    } // ApplyValue()
    #endregion

    #region Object Functions
    public int PossiblesCount()
    {
        return Possibles.Count;
    }

    public SNode DeepCopy()
    {
        SNode copy = (SNode)MemberwiseClone();

        copy.Possibles = new OrderedSet<int>();

        foreach (int value in this.Possibles)
            copy.Possibles.Add(value);

        return copy;
    }

    public string PrintPossibles()
    {
        string possiblesString = "";

        foreach (int value in Possibles)
            possiblesString += "(" + value.ToString() + "); ";

        return possiblesString;
    }
    #endregion

    #region Static Functions
    public static int[,] ConvertToOutput(SudokuBoard sBoard)
    {
        int setSize = sBoard.Sudoku.GetLength(0);
        int[,] board = new int[setSize, setSize];

        for (int yIndex = 0; yIndex < setSize; yIndex++)
        {
            for (int xIndex = 0; xIndex < setSize; xIndex++)
            {
                if (sBoard.Sudoku[xIndex, yIndex].Value > 0)
                    board[xIndex, yIndex] = sBoard.Sudoku[xIndex, yIndex].Value;
            }
        }

        return board;
    }
    #endregion
}
