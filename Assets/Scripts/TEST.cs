using UnityEngine;

public class TEST : MonoBehaviour
{
    void Start()
    {
        SNode[,] sudokuBoard = new SNode[9, 9];

        sudokuBoard[0, 0] = new SNode(4);
        sudokuBoard[1, 0] = new SNode(9);
        sudokuBoard[2, 0] = new SNode();

        sudokuBoard[3, 0] = new SNode();
        sudokuBoard[4, 0] = new SNode();
        sudokuBoard[5, 0] = new SNode();

        sudokuBoard[6, 0] = new SNode();
        sudokuBoard[7, 0] = new SNode();
        sudokuBoard[8, 0] = new SNode();



        sudokuBoard[0, 1] = new SNode();
        sudokuBoard[1, 1] = new SNode();
        sudokuBoard[2, 1] = new SNode(8);

        sudokuBoard[3, 1] = new SNode();
        sudokuBoard[4, 1] = new SNode(9);
        sudokuBoard[5, 1] = new SNode();

        sudokuBoard[6, 1] = new SNode(7);
        sudokuBoard[7, 1] = new SNode();
        sudokuBoard[8, 1] = new SNode();



        sudokuBoard[0, 2] = new SNode();
        sudokuBoard[1, 2] = new SNode();
        sudokuBoard[2, 2] = new SNode();

        sudokuBoard[3, 2] = new SNode();
        sudokuBoard[4, 2] = new SNode();
        sudokuBoard[5, 2] = new SNode();

        sudokuBoard[6, 2] = new SNode(1);
        sudokuBoard[7, 2] = new SNode(6);
        sudokuBoard[8, 2] = new SNode();



        sudokuBoard[0, 3] = new SNode(5);
        sudokuBoard[1, 3] = new SNode();
        sudokuBoard[2, 3] = new SNode();

        sudokuBoard[3, 3] = new SNode(2);
        sudokuBoard[4, 3] = new SNode();
        sudokuBoard[5, 3] = new SNode();

        sudokuBoard[6, 3] = new SNode();
        sudokuBoard[7, 3] = new SNode(4);
        sudokuBoard[8, 3] = new SNode();



        sudokuBoard[0, 4] = new SNode();
        sudokuBoard[1, 4] = new SNode();
        sudokuBoard[2, 4] = new SNode();

        sudokuBoard[3, 4] = new SNode();
        sudokuBoard[4, 4] = new SNode();
        sudokuBoard[5, 4] = new SNode(8);

        sudokuBoard[6, 4] = new SNode(2);
        sudokuBoard[7, 4] = new SNode();
        sudokuBoard[8, 4] = new SNode();



        sudokuBoard[0, 5] = new SNode(8);
        sudokuBoard[1, 5] = new SNode();
        sudokuBoard[2, 5] = new SNode();

        sudokuBoard[3, 5] = new SNode();
        sudokuBoard[4, 5] = new SNode();
        sudokuBoard[5, 5] = new SNode(5);

        sudokuBoard[6, 5] = new SNode(6);
        sudokuBoard[7, 5] = new SNode();
        sudokuBoard[8, 5] = new SNode();



        sudokuBoard[0, 6] = new SNode();
        sudokuBoard[1, 6] = new SNode(6);
        sudokuBoard[2, 6] = new SNode(2);

        sudokuBoard[3, 6] = new SNode();
        sudokuBoard[4, 6] = new SNode(1);
        sudokuBoard[5, 6] = new SNode();

        sudokuBoard[6, 6] = new SNode();
        sudokuBoard[7, 6] = new SNode();
        sudokuBoard[8, 6] = new SNode();


        sudokuBoard[0, 7] = new SNode();
        sudokuBoard[1, 7] = new SNode();
        sudokuBoard[2, 7] = new SNode(5);

        sudokuBoard[3, 7] = new SNode();
        sudokuBoard[4, 7] = new SNode(6);
        sudokuBoard[5, 7] = new SNode();

        sudokuBoard[6, 7] = new SNode();
        sudokuBoard[7, 7] = new SNode(9);
        sudokuBoard[8, 7] = new SNode();


        sudokuBoard[0, 8] = new SNode();
        sudokuBoard[1, 8] = new SNode(7);
        sudokuBoard[2, 8] = new SNode();

        sudokuBoard[3, 8] = new SNode();
        sudokuBoard[4, 8] = new SNode();
        sudokuBoard[5, 8] = new SNode(9);

        sudokuBoard[6, 8] = new SNode();
        sudokuBoard[7, 8] = new SNode();
        sudokuBoard[8, 8] = new SNode();


        int[,] solvedBoard = Solver.Solve(sudokuBoard, 9, 9);

        printBoard(solvedBoard);
	}

    void printBoard(int[,] sb)
    {
        for (int y = 0; y < 9; y++)
        {
            string line = "";
            for (int x = 0; x < 9; x++)
            {
                line += sb[x, y] + ", ";
            }

            print(line.Substring(0, line.Length - 2));
        }
    }
}
