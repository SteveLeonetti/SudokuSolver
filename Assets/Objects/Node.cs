using UnityEngine;

/// <summary>
/// Storage of each Cell/Node the user can type into.
/// </summary>

public class Node
{
    public GameObject textField; // Textbox for each Node
    private Node nextNode;

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

    public Node GetNextNode()
    {
        if (nextNode == null)
            return null;
        return nextNode;
    }

    public override string ToString()
    {
        return GetHashCode().ToString();
    }
}
