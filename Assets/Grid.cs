using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int xLength;
    private int yLength;

    private List<Node> grid;
    public Transform Camera;
    public int nodeSize;
    public GameObject NodePrefab;

    [HideInInspector] public Color defaultColor;
    [HideInInspector] public Color unwalkableColor;
    [HideInInspector] public Color openColor;
    [HideInInspector] public Color closedColor;
    [HideInInspector] public Color pathColor;
    [HideInInspector] public Color currColor;

    [HideInInspector] public int nodesPerSecond;
    private Coroutine _searchCoroutine;

    private Node startNode;
    private Node targetNode;

    int getPosInList(Vector2Int pos) {
        return (pos.x + (pos.y * xLength));
    }

    public void initGrid(int xLen, int yLen) {
        this.grid = new List<Node>();
        xLength = xLen; yLength = yLen;
        // Init the Grid
        for(int y = 0; y < yLength; y++) {
            for(int x = 0; x < xLength; x++) {
                Vector2Int posVec = new Vector2Int(x, y);
                Node n = Instantiate(NodePrefab, transform.position, transform.rotation).GetComponent<Node>();
                n.initNode(posVec, true, nodeSize, defaultColor);
                grid.Add(n);
            }
        }
    }

    public void clearGrid() {
        startNode = null;
        targetNode = null;
        foreach(Node n in grid) {
            n.isWalkable = true;
            n.canChangeColor = true;
            n.changeColor(defaultColor);
        }
    }

    public void resizeGrid(int xLen, int yLen) {
        clearGrid();
        this.xLength = xLen;
        this.yLength = yLen;
        initGrid(this.xLength, this.yLength);
    }

    public void startSearch() {
        Debug.Log("starting search");
        StopAllCoroutines();
        _searchCoroutine = StartCoroutine(findPath(startNode, targetNode));
    }

    public void resetGrid() {
        foreach(Node n in grid) {
            if(n.isWalkable) n.changeColor(defaultColor);
        }
        StopCoroutine(_searchCoroutine);
    }

    private void Start() {
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cam.gameObject.transform.position = new Vector3(((float) (xLength-1))/2.0f, 10f, ((float) (yLength-1))/2.0f +3);
        cam.orthographicSize = (Mathf.Max(xLength, yLength) / 2) + 4;
    }

    public Node getNodeFromList(Vector2Int posVec) {
        if((posVec.x < 0 | posVec.x >= xLength) | (posVec.y < 0 | posVec.y >= yLength)) return null;
        int listPos = getPosInList(posVec);
        if(listPos >= 0 && listPos <= (grid.Count - 1)) {
            return grid[listPos];
        } else {
            return null;
        }
    }

    //List.count is defined as the count of items in the list.  Same is Length
    List<Node> getNeighbours(Node n, bool moveDiag=true) {
        List<Node> neighbours = new List<Node>();

        if(moveDiag == false) {
            Vector2Int[] dirs = { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1) };
            foreach(Vector2Int dir in dirs) {
                Node neighbour = getNodeFromList(n.XYPos + dir);
                if(neighbour != null) neighbours.Add(neighbour);                
            }
            return neighbours;
        }

        for(int i = -1; i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                Vector2Int newPosXY = new Vector2Int(n.XYPos.x + i, n.XYPos.y + j);
                Node neighbour = getNodeFromList(newPosXY);
                if(neighbour != null) neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    public int getDistanceFromNode(Vector2Int a, Vector2Int b) {
        int xGap = Mathf.Abs(a.x - b.x);
        int yGap = Mathf.Abs(a.y - b.y);

        if(xGap <= yGap) {
            return (14 * xGap) + (10 * (yGap - xGap));
        }
        return (14 * yGap) + (10 * (xGap - yGap));
    }
    
    IEnumerator findPath(Node start, Node target) {
        List<Node> Open = new List<Node>(grid.Count);
        List<Node> Closed = new List<Node>(grid.Count);
        Open.Add(start);

        //foreach(Node curr in Open) {
        while(Open.Count > 0) {
            yield return new WaitForSecondsRealtime(1/(float)nodesPerSecond);
            Node curr = findNextNode(Open);
            Open.Remove(curr);
            Closed.Add(curr);

            if(curr == target) {
                //Found the target.
                List<Node> path = target.getPath(new List<Node>());
                path.Reverse();
                handleResults(path);
                yield break;
            }
            foreach(Node neighbour in getNeighbours(curr)) {                
                int newDistance = curr.gCost + getDistanceFromNode(curr.XYPos, neighbour.XYPos);

                if(neighbour.isWalkable == false | (Closed.Contains(neighbour) & newDistance >= neighbour.gCost)) {
                    //skip this neighbour.  
                    continue;
                }
                //calculate new costs if we go to neighbour from current
                //if new path to neighbour from current node is shorter or haven't been to 
                if(newDistance < neighbour.gCost || !Open.Contains(neighbour)){
                    neighbour.gCost = newDistance;
                    neighbour.parent = curr;
                    if(!Open.Contains(neighbour)){
                        Open.Add(neighbour);
                    }                
                }
            }
            foreach(Node n in Open) n.changeColor(openColor);
            foreach(Node n in Closed) n.changeColor(closedColor);
        }
        //Indicate failure, couldn't find a path.  Target might be unreachable.
        handleResults(null);
        yield break;
    }

    void handleResults(List<Node> path) {
        if(path != null) {
            Debug.Log("Found Path: ");
            foreach(Node n in path) {
                n.changeColor(pathColor);
                Debug.Log(string.Format("Node: " + n.XYPos));
            }
        }
        else {
            Debug.Log("Coudldn't find a path.");
        }
    }

    private Node findNextNode(List<Node> nodes) {
        Node rsf = null;
        foreach(Node curr in nodes) {
            if(rsf == null) rsf = curr;
            if(curr.getFCost() < rsf.getFCost() | (curr.getFCost() == rsf.getFCost() && curr.gCost < rsf.gCost)) {
                rsf = curr;
            }
        }
        return rsf;
    }

    public void setStartNode(Node node) {
        startNode = node;
    }
    
    public void setTargetNode(Node node) {
        targetNode = node;
        foreach(Node n in grid) n.hCost = getDistanceFromNode(n.XYPos, targetNode.XYPos);
    }

    public void genMaze() {
        StopAllCoroutines();
        StartCoroutine(generateMaze());
    }

    IEnumerator generateMaze() {
        clearGrid();
        Stack<Node> stack = new Stack<Node>();
        foreach(Node n in grid) {
            n.visited = false;
            n.isWalkable = false;
            n.changeColor(unwalkableColor);
            n.canChangeColor = false;
        }
        //iterative approach
        //instead of removing a wall, we will mark 2 blocks in the same direction as walkable.
        //1
        Node curr = grid[Random.Range(0, grid.Count)];
        curr.visited = true;
        stack.Push(curr);
        //2
        while(stack.Count != 0) {
            yield return new WaitForSecondsRealtime(0.02f);
            curr = stack.Pop(); //2.1
            List<Node> unvisitedNeighbours = findMazeNext(curr);

            if(unvisitedNeighbours.Count > 0) { //2.2
                stack.Push(curr); //2.2.1
                Node next = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)]; //2.2.2
                Node between = getNodeFromList(curr.XYPos + (next.XYPos - curr.XYPos) / 2);
                //If there is a node behind the next node, we mark it as walkable aswell.  
                Vector2Int dir = next.XYPos - curr.XYPos;
                makePath(next, between);
                stack.Push(next);
                next.visited = true;
            }
        }
        //generated maze.  

        yield break;
    }

    void makePath(Node n, Node between) {
        between.isWalkable = true;
        between.canChangeColor = true;
        between.changeColor(defaultColor);

        n.isWalkable = true;
        n.canChangeColor = true;
        n.changeColor(defaultColor);
    }

    List<Node> findMazeNext(Node n) {
        List<Node> neighbours = new List<Node>();
        Vector2Int[] dirs = { new Vector2Int(-2, 0), new Vector2Int(2, 0), new Vector2Int(0, -2), new Vector2Int(0, 2) };
        foreach(Vector2Int dir in dirs) {
            Node neighbour = getNodeFromList(n.XYPos + dir);
            if(neighbour != null) if(neighbour.visited == false) neighbours.Add(neighbour);
        }
        return neighbours;
    }
}

