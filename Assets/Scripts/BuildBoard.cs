using UnityEngine;

public class BuildBoard : MonoBehaviour
{
    public Canvas canvas;
    public GameObject SudokuNode;
    public GameObject SolveButton;
    public Camera Camera = null;
    public GameObject Control;

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    private Vector2 boardLocation = new Vector2(0, 0);
    private UINode[,] numBoxGrid = null;
    private int width = 9;
    private int height = 9;

    // Use this for initialization
    void Start()
    {
        // Draw Text Boxes
        UINode prevNode = new UINode();
        numBoxGrid = new UINode[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //
                //
                string name = "" + x + "x" + y + "y";
                GameObject go = Instantiate(SudokuNode);
                UINode newNode = new UINode(go);
                go.transform.SetParent(canvas.transform);
                go.name = name;
                numBoxGrid[x, y] = newNode;
                numBoxGrid[x, y].textField.transform.position = new Vector3(x / 3f - 1.35f, -(y / 3f) + 1.35f, 0);
                //numBoxGrid[x, y].textField.transform.position = new Vector3(x / 3f, -(y / 3f), 0);
                if (!(x == 0 && y == 0))
                {
                    prevNode.SetNextNode(numBoxGrid[x, y]);
                    numBoxGrid[x, y].SetPreviousNode(prevNode);
                }
                prevNode = numBoxGrid[x, y];
                //
                //
            }
        }

        Camera.transform.position = new Vector3(4 / 3f, -(8 / 3f), -10);
        //Camera.transform.position = new Vector3(numBoxGrid[4, 4].textField.transform.position.x, numBoxGrid[4, 4].textField.transform.position.y, -10);

        boardLocation = numBoxGrid[0, 0].textField.transform.position;  // retrieves location of board

        numBoxGrid[width - 1, height - 1].SetNextNode(numBoxGrid[0, 0]);
        numBoxGrid[0, 0].SetPreviousNode(numBoxGrid[width - 1, height - 1]);

        // Build Button


        // Build Controller
        Control = GameObject.FindGameObjectWithTag("GameController");
        Control.AddComponent<Controller>();
        Control.GetComponent<Controller>().SendGrid(numBoxGrid);
	}

    public static void GUIDrawRect(Rect divider, Color color)
    {
            _staticRectTexture = new Texture2D(1, 1);

        if (_staticRectStyle == null)
            _staticRectStyle = new GUIStyle();

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(divider, GUIContent.none, _staticRectStyle);
    }

    private void OnGUI()
    {
        // Build Dividers
        boardLocation = new Vector2(1613, 774);
        Rect leftline = new Rect(boardLocation + new Vector2(192, 0), new Vector2(10, 605));
        GUIDrawRect(leftline, Color.white);
        Rect rightline = new Rect(boardLocation + new Vector2(406, 0), new Vector2(10, 605));
        GUIDrawRect(rightline, Color.white);
        Rect upperline = new Rect(boardLocation + new Vector2(0, 188), new Vector2(605, 10));
        GUIDrawRect(upperline, Color.white);
        Rect lowerline = new Rect(boardLocation + new Vector2(0, 406), new Vector2(605, 10));
        GUIDrawRect(lowerline, Color.white);
    }
}