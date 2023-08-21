using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    #region Singelton

    public static Pathfinding Instance { get; private set; }

    #endregion

    #region Inspector

    [SerializeField] private Grid grid;

    #endregion

    #region Private Parameters

    private Vector3 startPosition;
    private List<Node> path = new List<Node>();

    #endregion

    private void Awake() => Instance ??= this;

    public void SetStartPos(Vector3 pos) => startPosition = pos;

    public List<Node> FindPath(Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost ||
                    OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == targetNode)
                SetPath(startNode, targetNode);

            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.isObstacle || ClosedList.Contains(NeighborNode))
                    continue;

                int MoveCost = CurrentNode.gCost + ManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = ManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                        OpenList.Add(NeighborNode);
                }
            }
        }

        return path;
    }

    private void SetPath(Node startingNode, Node endNode)
    {
        List<Node> thePath = new List<Node>();
        Node CurrentNode = endNode;

        while (CurrentNode != startingNode)
        {
            thePath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        path.Reverse();
        grid.ThePath = thePath;
        path = thePath;
    }

    private int ManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);
        return ix + iy;
    }

    public Vector3 AvailableRandomPoint() => grid.RandomPoint();
}