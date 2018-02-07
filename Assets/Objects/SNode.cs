public class SNode
{
    public int value = 0;
    public bool[] possibles;
    public int possRem = 9;
    public bool isSolved;

    public SNode[] row;
    public SNode[] column;
    public SNode[] box;

    public SNode()
    {
        value = 0;
        possibles = new bool[] {true, true, true, true, true, true, true, true, true};
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
        value = v;
        possibles = null;
        possRem = 0;
        isSolved = true;
    }

    public void SetRow(SNode[] r)
    {
        row = r;
    }

    public void SetColumn(SNode[] c)
    {
        column = c;
    }

    public void SetBox(SNode[] b)
    {
        box = b;
    }

    public bool IncludesPossibles(bool[] a)
    {

        return true;
    }

    public static int[,] ConvertToOutput(SNode[,] snode, int width, int height)
    {
        int[,] board = new int[width, height];
        
        for (int yIndex = 0; yIndex < height; yIndex++)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                board[xIndex, yIndex] = snode[xIndex, yIndex].value;
            }
        }

        return board;
    }

    public string PrintPossibles()
    {
        string possiblesString = "";

        for (int i = 0; i < 9; i++)
        {
            possiblesString += "(" + i + ": " + possibles[i].ToString() + "); ";
        }

        return possiblesString;
    }
}
