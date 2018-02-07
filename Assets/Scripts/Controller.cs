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

    public void SendGrid(Node[,] grid)
    {
        gridNodes = grid;
    }

    public void PrintGrid(int[,] grid)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                //
                //
                gridNodes[x, y].textField.GetComponent<InputField>().text = grid[x, y].ToString();
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
                    focusedNode = focusedNode.GetNextNode();
                    focusedBox = focusedNode.textField;
                    focusedBox.GetComponent<InputField>().Select();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
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
                        //
                        //
                    }
                }

                PrintGrid(Solver.Solve(sudokuBoard, 9, 9));
            }
        }

        // Exempt from code
    }
}
