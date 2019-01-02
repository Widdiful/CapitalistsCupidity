using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{ 
    public Vector2 gridWorldSize;
    float nodeRadius = 0.15f;
    public Node[,] worldGrid;

    public float nodeDiameter;
    int gridSizeX, gridSizeY;
    Vector3 worldBottomLeft;


    private void Start()
    {
        gridWorldSize = new Vector2(transform.localScale.x - 1, transform.localScale.z - 1); //Sizes the grid depending on the size of the floor

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        createGrid();
    }

    public int maxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    public Node nodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
        float percentY = (worldPos.z - transform.position.z) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return worldGrid[x, y];

    }

    public List<Node> getNeighbours(Node n)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = n.gridX + x;
                int checkY = n.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(worldGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }



    public void updateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);

                Collider[] hit = Physics.OverlapSphere(worldPoint, nodeRadius);

                bool walkable = true;

                foreach (Collider h in hit)
                {
                    if (h.tag == "unwalkable")
                    {
                        walkable = false;
                    }

                }
                worldGrid[x, y].walkable = walkable;
            }
        }
    }

    void createGrid()
    {
        worldGrid = new Node[gridSizeX, gridSizeY];

        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 //Gives bottom left corner of world
            - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);

                Collider[] hit = Physics.OverlapSphere(worldPoint, nodeRadius);

                bool walkable = true;

                foreach (Collider h in hit)
                {
                    if(h.tag == "unwalkable")
                    {
                        walkable = false;
                    }

                }
                worldGrid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
}
