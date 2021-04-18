using System;
using UnityEngine;

/// <summary>
/// Storage of each Cell/Node the user can type into.
/// </summary>

public class UINode
{
    public GameObject textField; // Textbox for each Node
    private UINode nextNode;
    private UINode previousNode;

    public UINode()
    {

    }
    public UINode(GameObject n)
    {
        textField = n;
    }

    public void SetNextNode(UINode n)
    {
        nextNode = n;
    }
    
    public void SetPreviousNode(UINode n)
    {
        previousNode = n;
    }

    public UINode GetNextNode()
    {
        if (nextNode == null)
            return null;
        return nextNode;
    }

    public UINode GetPreviousNode()
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
