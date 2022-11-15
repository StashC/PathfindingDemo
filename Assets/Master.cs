using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {
    public bool run;
    public bool reset;
    
    private Grid _theGrid;
    public int xLength;
    public int yLength;

    public Color defaultColor;
    public Color unwalkableColor;
    public Color openColor;
    public Color closedColor;
    public Color pathColor;
    public Color currColor;
    public Color startColor;
    public Color targetColor;

    public Vector2Int start;
    public Vector2Int target;
    public GameObject NodePrefab;

    private Camera cam;
   
    // Start is called before the first frame update
    void Start() {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _theGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        setGridColors();
        _theGrid.initGrid(NodePrefab, xLength, yLength);
        setStartNode(_theGrid.getNodeFromList(start));
        setTargetNode(_theGrid.getNodeFromList(target));
    }

    void setGridColors() {
        _theGrid.defaultColor = defaultColor;
        _theGrid.unwalkableColor = unwalkableColor;
        _theGrid.openColor = openColor;
        _theGrid.closedColor = closedColor;
        _theGrid.pathColor = pathColor;
        _theGrid.currColor = currColor;
    }

    void toggleWalkable(Node node) {
        node.isWalkable = !node.isWalkable;
        if(node.isWalkable) node.changeColor(defaultColor); else node.changeColor(unwalkableColor);
    }

    void setStartNode(Node startNode) {
        _theGrid.setStartNode(startNode);
        startNode.changeColor(startColor);
        startNode.canChangeColor = false;
    }

    void setTargetNode(Node targetNode) {
        _theGrid.setTargetNode(targetNode);
        targetNode.changeColor(targetColor);
        targetNode.canChangeColor = false;
    }

    private void Update() {
        if(run) {
            _theGrid.startSearch();
            run = false;
        }

        if(reset) {
            _theGrid.resetGrid();
            reset = false;
        }

        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Node clickedNode = getNodeClicked(mousePos);
            toggleWalkable(clickedNode);
        }
    }

    Node getNodeClicked(Vector3 mouseWorldPos) {
        Vector2Int nodePos = new Vector2Int((int) mouseWorldPos.x, (int) mouseWorldPos.z);
        Debug.Log("Clicked Node: " + nodePos + " world pos: " + mouseWorldPos);
        return _theGrid.getNodeFromList(nodePos);
    }
}
