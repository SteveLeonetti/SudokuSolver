using UnityEngine;

public class BuildBoard : MonoBehaviour
{
    public Canvas canvas;
    public GameObject SudokuNode;
    public GameObject SolveButton;
    public Camera Camera = null;
    public GameObject Control;
    
    private Node[,] numBoxGrid = null;
    private int width = 9;
    private int height = 9;

    // Use this for initialization
    void Start()
    {
        Node prevNode = new Node();
        Camera.transform.position = new Vector3(4 / 3f, -(8 / 3f), -10);
        numBoxGrid = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //
                //
                string name = "" + x + "x" + y + "y";
                GameObject go = Instantiate(SudokuNode);
                Node newNode = new Node(go);
                go.transform.SetParent(canvas.transform);
                go.name = name;
                numBoxGrid[x, y] = newNode;
                numBoxGrid[x, y].textField.transform.position = new Vector3(x / 3f, -(y / 3f), 0);
                if (!(x == 0 && y == 0))
                    prevNode.SetNextNode(numBoxGrid[x, y]);
                prevNode = numBoxGrid[x, y];
                //
                //
            }
        }

        numBoxGrid[width - 1, height - 1].SetNextNode(numBoxGrid[0, 0]);

        // Build Button


        // Build Controller
        Control = GameObject.FindGameObjectWithTag("GameController");
        Control.AddComponent<Controller>();
        Control.GetComponent<Controller>().SendGrid(numBoxGrid);
        
        
	}
}
