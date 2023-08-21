using UnityEngine;

public class Node
{
    public Node ParentNode;

    public int gridX;
    public int gridY;

    public bool isObstacle;
    public Vector3 position;

    public int gCost;
    public int hCost;

    public int FCost
    {
        get => gCost + hCost;
    }

    public Node(bool IsObstacle, Vector3 Pos, int GridX, int GridY)
    {
        isObstacle = IsObstacle;
        position = Pos;
        gridX = GridX;
        gridY = GridY;
    }
}