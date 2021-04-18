public class Test
{
    public static void Start(Controller c)
    {
        SNode[,] sudokuBoard = new SNode[9, 9];

        ////////////////////////////////////// 1
        
        sudokuBoard[0, 0] = SetNode(5);

        sudokuBoard[1, 0] = SetNode(69);

        sudokuBoard[2, 0] = SetNode(8);


        sudokuBoard[3, 0] = SetNode(4);

        sudokuBoard[4, 0] = SetNode(2);

        sudokuBoard[5, 0] = SetNode(679);


        sudokuBoard[6, 0] = SetNode(79);

        sudokuBoard[7, 0] = SetNode(3);

        sudokuBoard[8, 0] = SetNode(1);

        ////////////////////////////////////// 2

        sudokuBoard[0, 1] = SetNode(37);

        sudokuBoard[1, 1] = SetNode(2349);

        sudokuBoard[2, 1] = SetNode(2479);


        sudokuBoard[3, 1] = SetNode(79);

        sudokuBoard[4, 1] = SetNode(1);

        sudokuBoard[5, 1] = SetNode(35);


        sudokuBoard[6, 1] = SetNode(6);

        sudokuBoard[7, 1] = SetNode(5789);

        sudokuBoard[8, 1] = SetNode(258);

        ////////////////////////////////////// 3

        sudokuBoard[0, 2] = SetNode(1);

        sudokuBoard[1, 2] = SetNode(2369);

        sudokuBoard[2, 2] = SetNode(2679);


        sudokuBoard[3, 2] = SetNode(679);

        sudokuBoard[4, 2] = SetNode(8);

        sudokuBoard[5, 2] = SetNode(35);


        sudokuBoard[6, 2] = SetNode(2579);

        sudokuBoard[7, 2] = SetNode(4);

        sudokuBoard[8, 2] = SetNode(25);

        ////////////////////////////////////// 4

        sudokuBoard[0, 3] = SetNode(89);

        sudokuBoard[1, 3] = SetNode(256);

        sudokuBoard[2, 3] = SetNode(26);


        sudokuBoard[3, 3] = SetNode(16789);

        sudokuBoard[4, 3] = SetNode(79);

        sudokuBoard[5, 3] = SetNode(16789);


        sudokuBoard[6, 3] = SetNode(3);

        sudokuBoard[7, 3] = SetNode(15678);

        sudokuBoard[8, 3] = SetNode(4);

        ////////////////////////////////////// 5

        sudokuBoard[0, 4] = SetNode(89);

        sudokuBoard[1, 4] = SetNode(56);

        sudokuBoard[2, 4] = SetNode(1);


        sudokuBoard[3, 4] = SetNode(3);

        sudokuBoard[4, 4] = SetNode(4);

        sudokuBoard[5, 4] = SetNode(6789);


        sudokuBoard[6, 4] = SetNode(257);

        sudokuBoard[7, 4] = SetNode(5678);

        sudokuBoard[8, 4] = SetNode(258);

        ////////////////////////////////////// 6

        sudokuBoard[0, 5] = SetNode(4);

        sudokuBoard[1, 5] = SetNode(7);

        sudokuBoard[2, 5] = SetNode(3);


        sudokuBoard[3, 5] = SetNode(2);

        sudokuBoard[4, 5] = SetNode(5);

        sudokuBoard[5, 5] = SetNode(168);


        sudokuBoard[6, 5] = SetNode(18);

        sudokuBoard[7, 5] = SetNode(168);

        sudokuBoard[8, 5] = SetNode(9);

        ////////////////////////////////////// 7

        sudokuBoard[0, 6] = SetNode(6);

        sudokuBoard[1, 6] = SetNode(8);

        sudokuBoard[2, 6] = SetNode(59);


        sudokuBoard[3, 6] = SetNode(19);

        sudokuBoard[4, 6] = SetNode(3);

        sudokuBoard[5, 6] = SetNode(2);


        sudokuBoard[6, 6] = SetNode(4);

        sudokuBoard[7, 6] = SetNode(159);

        sudokuBoard[8, 6] = SetNode(7);

        ////////////////////////////////////// 8

        sudokuBoard[0, 7] = SetNode(37);

        sudokuBoard[1, 7] = SetNode(349);

        sudokuBoard[2, 7] = SetNode(479);


        sudokuBoard[3, 7] = SetNode(5);

        sudokuBoard[4, 7] = SetNode(79);

        sudokuBoard[5, 7] = SetNode(18);


        sudokuBoard[6, 7] = SetNode(18);

        sudokuBoard[7, 7] = SetNode(2);

        sudokuBoard[8, 7] = SetNode(6);

        ////////////////////////////////////// 9

        sudokuBoard[0, 8] = SetNode(2);

        sudokuBoard[1, 8] = SetNode(1);

        sudokuBoard[2, 8] = SetNode(579);


        sudokuBoard[3, 8] = SetNode(789);

        sudokuBoard[4, 8] = SetNode(6);

        sudokuBoard[5, 8] = SetNode(4);


        sudokuBoard[6, 8] = SetNode(59);

        sudokuBoard[7, 8] = SetNode(589);

        sudokuBoard[8, 8] = SetNode(3);


        c.PrintGrid(Solver.Solve(sudokuBoard, c));
    }

    static SNode SetNode(int list)
    {
        SNode sNode = new SNode(new bool[] { false, false, false, false, false, false, false, false, false });

        if (list.ToString().Length == 1)
        {
            sNode.value = list;
            sNode.isSolved = true;
            sNode.possibles = null;
            sNode.possRem = 0;
        }
        else
        {
            char[] possibles = list.ToString().ToCharArray();

            foreach (char c in possibles)
            {
                int i = 0;
                int.TryParse(c.ToString(), out i);

                sNode.possibles[i - 1] = true;
            }

            sNode.value = 0;
            sNode.isSolved = false;
            sNode.possRem = possibles.Length;
        }

        return sNode;
    }

    static void printBoard(int[,] sb)
    {
        for (int y = 0; y < 9; y++)
        {
            string line = "";
            for (int x = 0; x < 9; x++)
            {
                line += sb[x, y] + ", ";

                //print(line.Substring(0, line.Length - 2));

            }
        }
    }
}