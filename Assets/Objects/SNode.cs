using System.Collections.Generic;

public class SNode
{
    public int rowIndex = 0;
    public int columnIndex = 0;

    public int value = 0;
    public bool[] possibles;
    public int possRem = 9;
    public bool isSolved;

    public Dictionary<int, SNode> Row { get; set; }

    public Dictionary<int, SNode> Column { get; set; }

    public Dictionary<int, SNode> Box { get; set; }

    #region Constructors
    public SNode()
    {
        value = 0;
        possibles = new bool[] { true, true, true, true, true, true, true, true, true };
        possRem = 9;
        isSolved = false;
    }

    public SNode(bool[] poss)
    {
        value = 0;
        possibles = poss;

        foreach (bool b in possibles)
        {
            if (b)
                possRem++;
        }

        isSolved = false;
    }

    public SNode(int v)
    {
        if (v >=1 && v <= 9)
        {
            value = v;
            possibles = null;
            possRem = 0;
            isSolved = true;
        }
    }
    #endregion

    #region Object Functions
    public string PrintPossibles()
    {
        string possiblesString = "";

        for (int i = 0; i < 9; i++)
        {
            possiblesString += "(" + i + ": " + possibles[i].ToString() + "); ";
        }

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
                if (sBoard.Sudoku[xIndex, yIndex].value > 0)
                    board[xIndex, yIndex] = sBoard.Sudoku[xIndex, yIndex].value;
            }
        }

        return board;
    }
    #endregion
}
