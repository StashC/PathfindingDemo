using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    [Header("Shared")]
    public Vector2Int XYPos;
    private Color nodeColor;
    public bool canChangeColor;
    private int size;
    
    //Astar
    [HideInInspector]
    public int gCost;
    public int hCost;
    public int getFCost(){ return gCost + hCost; }
    public bool isWalkable;
    public Node parent;

    //MazeGeneration 
    public bool visited;

    public Node initNode(Vector2Int XYPos, bool isWalkable, int nodeSize, Color color) {
        this.XYPos = XYPos;
        this.isWalkable = isWalkable;
        this.parent = null;
        this.size = nodeSize;
        this.canChangeColor = true;

        setWorldPos();
        changeColor(color);
        return this;
    }

    private void setWorldPos() {
        transform.position = new Vector3(XYPos.x + 0.5f, 0, XYPos.y + 0.5f);
    }

    public List<Node> getPath(List<Node> path) {
        path.Add(this);
        if(parent == null) {
            return path;
        }

        return parent.getPath(path);
    }

    public void changeColor(Color color) {
        if(canChangeColor) {
            nodeColor = color;
            //set the color of the renderer or whatever
            GetComponent<Renderer>().material.color = nodeColor;
        }
    }

}
