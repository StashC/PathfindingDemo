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

    private Vector2Int start;
    private Vector2Int target;
    public GameObject NodePrefab;

    // "ToggleWalkable", "SetStart", "SetTarget"
    [HideInInspector]
    public string currButton = "ToggleWalkable";

    private Camera cam;

    public 
   
    // Start is called before the first frame update
    void Start() {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _theGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        setGridColors();
        _theGrid.initGrid(NodePrefab, xLength, yLength);
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
        if(node == null) return;
        node.isWalkable = !node.isWalkable;
        if(node.isWalkable) node.changeColor(defaultColor); else node.changeColor(unwalkableColor);
    }

    void setStartNode(Node startNode) {
        if(startNode == null) return;
        if(start != null) {
            Node old = _theGrid.getNodeFromList(start);
            old.canChangeColor = true;
            old.changeColor(defaultColor);
        }

        this.start = startNode.XYPos;
        _theGrid.setStartNode(startNode);
        startNode.changeColor(startColor);
        startNode.canChangeColor = false;
    }

    void setTargetNode(Node targetNode) {
        if(targetNode == null) return;
        if(target != null) {
            Node old = _theGrid.getNodeFromList(target);
            old.canChangeColor = true;
            old.changeColor(defaultColor);
        }

        this.target = targetNode.XYPos;
        _theGrid.setTargetNode(targetNode);
        targetNode.changeColor(targetColor);
        targetNode.canChangeColor = false;
    }

    public void clearGrid() {
        _theGrid.clearGrid();
    }

    public void startSearch() {
        _theGrid.startSearch();
    }

    public void resetSearch() {
        _theGrid.resetGrid();
    }

        
    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            handleClick();
        }
    }
    private void handleClick() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        try {
            Node clickedNode = getNodeClicked(mousePos);
            Debug.Log(clickedNode.XYPos);
            Debug.Log(currButton);
            if(clickedNode == null) return;
            switch(currButton) {
                case "ToggleWalkable":
                    toggleWalkable(clickedNode);
                    break;
                case "SetStart":
                    setStartNode(clickedNode);
                    break;
                case "SetTarget":
                    setTargetNode(clickedNode);
                    break;
                default:
                    break;
            }
        } catch {
            // Debug.LogError("Couldnt get Node @ Pos: " + mousePos);
            return;
        }
        
    }
    Node getNodeClicked(Vector3 mouseWorldPos) {
        Vector2Int nodePos = new Vector2Int((int) mouseWorldPos.x, (int) mouseWorldPos.z);
        // Debug.Log("Clicked Node: " + nodePos + " world pos: " + mouseWorldPos);
        return _theGrid.getNodeFromList(nodePos);
    }
}
