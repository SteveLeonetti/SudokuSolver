using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private GameObject focusedBox;
    private UINode focusedNode;
    private UINode[,] gridNodes;
    private bool leftMouseClicked = false;
    private bool shiftPressed = false;

    public void SendGrid(UINode[,] grid)
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
                foreach (UINode node in gridNodes)
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
                RunSolver();
            }
        } //
    
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            shiftPressed = false;
    
        // Exempt from code, don't put code here
    }

    public void RunSolver()
    {
        OrderedSet<int> sudokuNumbers = new OrderedSet<int>();
        foreach (int value in new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            sudokuNumbers.Add(value);

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
                SNode sNode;
                int value;
                if (int.TryParse(gridNodes[x, y].textField.GetComponent<InputField>().text, out value))
                    sNode = new SNode(value);
                else
                    sNode = new SNode(sudokuNumbers);

                sNode.rowIndex = y;
                sNode.columnIndex = x;

                sudokuBoard[x, y] = sNode;
                //
                //
            }
        }

        int[,] grid = Solver.Solve(sudokuBoard, this);
        PrintGrid(grid);
    }

    public void PrintStatement(string statement)
    {
        print(statement);
    }
}
