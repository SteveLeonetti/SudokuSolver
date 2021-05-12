public class Test
{
    static SNode[,] sudokuBoard;
    static OrderedSet<int> sudokuNumbers;
    static Controller controller;

    public static void Start(Controller c)
    {
        controller = c;
        sudokuNumbers = new OrderedSet<int>();
        foreach (int value in new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            sudokuNumbers.Add(value);

        sudokuBoard = new SNode[9, 9];

        ////////////////////////////////////// 1
        
        sudokuBoard[0, 0] = SetNode(8);

        sudokuBoard[1, 0] = SetNode();

        sudokuBoard[2, 0] = SetNode();


        sudokuBoard[3, 0] = SetNode();

        sudokuBoard[4, 0] = SetNode();

        sudokuBoard[5, 0] = SetNode();


        sudokuBoard[6, 0] = SetNode();

        sudokuBoard[7, 0] = SetNode();

        sudokuBoard[8, 0] = SetNode();

        ////////////////////////////////////// 2

        sudokuBoard[0, 1] = SetNode();

        sudokuBoard[1, 1] = SetNode();

        sudokuBoard[2, 1] = SetNode(3);


        sudokuBoard[3, 1] = SetNode(6);

        sudokuBoard[4, 1] = SetNode();

        sudokuBoard[5, 1] = SetNode();


        sudokuBoard[6, 1] = SetNode();

        sudokuBoard[7, 1] = SetNode();

        sudokuBoard[8, 1] = SetNode();

        ////////////////////////////////////// 3

        sudokuBoard[0, 2] = SetNode();

        sudokuBoard[1, 2] = SetNode(7);

        sudokuBoard[2, 2] = SetNode();


        sudokuBoard[3, 2] = SetNode();

        sudokuBoard[4, 2] = SetNode(9);

        sudokuBoard[5, 2] = SetNode();


        sudokuBoard[6, 2] = SetNode(2);

        sudokuBoard[7, 2] = SetNode();

        sudokuBoard[8, 2] = SetNode();

        ////////////////////////////////////// 4

        sudokuBoard[0, 3] = SetNode();

        sudokuBoard[1, 3] = SetNode(5);

        sudokuBoard[2, 3] = SetNode();


        sudokuBoard[3, 3] = SetNode();

        sudokuBoard[4, 3] = SetNode();

        sudokuBoard[5, 3] = SetNode(7);


        sudokuBoard[6, 3] = SetNode();

        sudokuBoard[7, 3] = SetNode();

        sudokuBoard[8, 3] = SetNode();

        ////////////////////////////////////// 5

        sudokuBoard[0, 4] = SetNode();

        sudokuBoard[1, 4] = SetNode();

        sudokuBoard[2, 4] = SetNode();


        sudokuBoard[3, 4] = SetNode();

        sudokuBoard[4, 4] = SetNode(4);

        sudokuBoard[5, 4] = SetNode(5);


        sudokuBoard[6, 4] = SetNode(7);

        sudokuBoard[7, 4] = SetNode();

        sudokuBoard[8, 4] = SetNode();

        ////////////////////////////////////// 6

        sudokuBoard[0, 5] = SetNode();

        sudokuBoard[1, 5] = SetNode();

        sudokuBoard[2, 5] = SetNode();


        sudokuBoard[3, 5] = SetNode(1);

        sudokuBoard[4, 5] = SetNode();

        sudokuBoard[5, 5] = SetNode();


        sudokuBoard[6, 5] = SetNode();

        sudokuBoard[7, 5] = SetNode(3);

        sudokuBoard[8, 5] = SetNode();

        ////////////////////////////////////// 7

        sudokuBoard[0, 6] = SetNode();

        sudokuBoard[1, 6] = SetNode();

        sudokuBoard[2, 6] = SetNode(1);


        sudokuBoard[3, 6] = SetNode();

        sudokuBoard[4, 6] = SetNode();

        sudokuBoard[5, 6] = SetNode();


        sudokuBoard[6, 6] = SetNode();

        sudokuBoard[7, 6] = SetNode(6);

        sudokuBoard[8, 6] = SetNode(8);

        ////////////////////////////////////// 8

        sudokuBoard[0, 7] = SetNode();

        sudokuBoard[1, 7] = SetNode();

        sudokuBoard[2, 7] = SetNode(8);


        sudokuBoard[3, 7] = SetNode(5);

        sudokuBoard[4, 7] = SetNode();

        sudokuBoard[5, 7] = SetNode();


        sudokuBoard[6, 7] = SetNode();

        sudokuBoard[7, 7] = SetNode(1);

        sudokuBoard[8, 7] = SetNode();

        ////////////////////////////////////// 9

        sudokuBoard[0, 8] = SetNode();

        sudokuBoard[1, 8] = SetNode(9);

        sudokuBoard[2, 8] = SetNode();


        sudokuBoard[3, 8] = SetNode();

        sudokuBoard[4, 8] = SetNode();

        sudokuBoard[5, 8] = SetNode();


        sudokuBoard[6, 8] = SetNode(4);

        sudokuBoard[7, 8] = SetNode();

        sudokuBoard[8, 8] = SetNode();


        SetIndices();

        c.PrintGrid(Solver.Solve(sudokuBoard, c));
    }

    static SNode SetNode()
    {
        return new SNode(sudokuNumbers);
    }

    static SNode SetNode(int list)
    {
        char[] possiblesList = list.ToString().ToCharArray();
        OrderedSet<int> possibles = new OrderedSet<int>();

        foreach (char c in possiblesList)
        {
            int i = 0;
            int.TryParse(c.ToString(), out i);

            possibles.Add(i);
        }

        if (possibles.Count == 0)
            return new SNode(sudokuNumbers);
        else if (possibles.Count == 1)
            return new SNode(possibles[0]);
        else
            return new SNode(possibles);
    }

    static void SetIndices()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                sudokuBoard[column, row].columnIndex = column;
                sudokuBoard[column, row].rowIndex = row;
            }
        }
    }

    static void printBoard(int[,] sb)
    {
        for (int y = 0; y < 9; y++)
        {
            string line = "";
            for (int x = 0; x < 9; x++)
            {
                line += sb[x, y] + ", ";

                controller.PrintStatement(line.Substring(0, line.Length - 2));

            }
        }
    }
}