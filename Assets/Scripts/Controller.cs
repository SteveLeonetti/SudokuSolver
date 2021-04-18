using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private GameObject focusedBox;
    private Node focusedNode;
    private Node[,] gridNodes;
    private bool leftMouseClicked = false;
    private bool shiftPressed = false;

    public void SendGrid(Node[,] grid)
    {
        gridNodes = grid;
    }

    public void PrintGrid(int[,] grid)
    {
        for (int column = 0; column < grid.GetLength(1); column++)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                //
                //
                if (grid[row, column] > 0)
                    gridNodes[row, column].textField.GetComponent<InputField>().text = grid[row, column].ToString();
                //
                //
            }
        }
    }

    void Update()
    {
        if (gridNodes != null)
        {
            // Next Update Cycle, check node
            if (leftMouseClicked)
            {
                foreach (Node node in gridNodes)
                {
                    if (node.textField.GetComponent<InputField>().isFocused)
                    {
                        focusedNode = node;
                        focusedBox = focusedNode.textField;
                        focusedBox.GetComponent<InputField>().Select();
                        break;
                    }
                }

                leftMouseClicked = false;
            }

            // Check for key presses

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                shiftPressed = true;

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                shiftPressed = false;

            if (Input.GetMouseButtonDown(0))
            {
                leftMouseClicked = true;
            }

            // Checks for Tab pressed
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (focusedBox == null)
                {
                    focusedNode = gridNodes[0, 0];
                    focusedBox = focusedNode.textField;
                    focusedBox.GetComponent<InputField>().Select();
                }
                else
                {
                    if (shiftPressed)
                    {
                        focusedNode = focusedNode.GetPreviousNode();
                        focusedBox = focusedNode.textField;
                        focusedBox.GetComponent<InputField>().Select();
                    }
                    else
                    {
                        focusedNode = focusedNode.GetNextNode();
                        focusedBox = focusedNode.textField;
                        focusedBox.GetComponent<InputField>().Select();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (shiftPressed)
                {
                    Test.Start(this);

                    return;
                }

                SNode[,] sudokuBoard = new SNode[9, 9];

                // Fill board double loop
                for (int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        //
                        //
                        if (gridNodes[x, y].textField.GetComponent<InputField>().text.Equals(""))
                            sudokuBoard[x, y] = new SNode();
                        else
                        {
                            int value;
                            if (int.TryParse(gridNodes[x, y].textField.GetComponent<InputField>().text, out value))
                                sudokuBoard[x, y] = new SNode(value);
                            else
                                sudokuBoard[x, y] = new SNode();
                        }

                        sudokuBoard[x, y].rowIndex = y;
                        sudokuBoard[x, y].columnIndex = x;
                        //
                        //
                    }
                }

                int[,] grid = Solver.Solve(sudokuBoard, this);
                PrintGrid(grid);
            }
        }

        // Exempt from code, don't put code here
    }

    public void PrintStatement(string statement)
    {
        print(statement);
    }
}
