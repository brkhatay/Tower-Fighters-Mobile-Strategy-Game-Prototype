using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region Inspector

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField, HideInInspector] private Vector2 gridWorldSize;

    [SerializeField] private float nodeRadius = .5f;
    [SerializeField] private float distanceBetweenNodes = .5f;

    [Space] [Header("Mask")] [SerializeField]
    private LayerMask mask;

    #endregion

    private Node[,] NodeArray;
    public List<Node> ThePath;

    private float fNodeDiameter => nodeRadius * 2;
    private int gridSizeX, gridSizeY;

    private void Start() => MakeGrid();

    public void MakeGrid()
    {
        if (meshRenderer != null)
            gridWorldSize = new Vector2(meshRenderer.bounds.size.x, meshRenderer.bounds.size.z);

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / fNodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / fNodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        NodeArray = new Node[gridSizeX, gridSizeY];

        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                             Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                bool obstacle = true;

                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * fNodeDiameter + nodeRadius);

                if (Physics.CheckSphere(worldPoint, nodeRadius, mask))
                    obstacle = false;

                NodeArray[x, y] = new Node(obstacle, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(NodeArray[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float ixPos = ((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float iyPos = ((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((gridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((gridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }

    public Vector3 RandomPoint()
    {
        List<Vector3> tempList = new List<Vector3>();
        foreach (Node n in NodeArray)
        {
            if (n.isObstacle)
                tempList.Add(n.position);
        }

        return tempList[Random.Range(0, tempList.Count)];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (gridWorldSize.x > 0)
        {
            Gizmos.DrawWireCube(transform.position,
                new Vector3(gridWorldSize.x, 1,
                    gridWorldSize.y));
        }

        if (NodeArray != null)
        {
            foreach (Node n in NodeArray)
            {
                if (n.isObstacle)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.red;

                if (ThePath != null)
                    if (ThePath.Contains(n))
                        Gizmos.color = Color.blue;

                Gizmos.DrawCube(n.position,
                    Vector3.one * (fNodeDiameter - distanceBetweenNodes));
            }
        }
    }
#endif
}