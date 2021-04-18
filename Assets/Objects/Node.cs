using System;
using UnityEngine;

/// <summary>
/// Storage of each Cell/Node the user can type into.
/// </summary>

public class Node
{
    public GameObject textField; // Textbox for each Node
    private Node nextNode;
    private Node previousNode;

    public Node()
    {

    }
    public Node(GameObject n)
    {
        textField = n;
    }

    public void SetNextNode(Node n)
    {
        nextNode = n;
    }
    
    public void SetPreviousNode(Node n)
    {
        previousNode = n;
    }

    public Node GetNextNode()
    {
        if (nextNode == null)
            return null;
        return nextNode;
    }

    public Node GetPreviousNode()
    {
        if (previousNode == null)
            return null;
        return previousNode;
    }

    public override string ToString()
    {
        return GetHashCode().ToString();
    }
}
