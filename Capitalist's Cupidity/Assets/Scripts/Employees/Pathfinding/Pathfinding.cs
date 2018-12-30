﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Grid grid;

    Employee emp;
    public Vector3 target;

    public List<Node> newPath;

    public bool foundPath;

    private void Start()
    {
        emp = GetComponent<Employee>();
    }


    private void Update()
    {
        if (emp.pathComplete)
        {
            grid = emp.getCurrentGrid[emp.currentFloor];
            newPath = new List<Node>();
            target = target = emp.targetPos;
            emp.pathComplete = false;
            emp.currentPathPoint = 0;
            findPath(emp.transform.position, target);
        }
    }


    public void findPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.nodeFromWorldPoint(startPos);
        Node targetNode = grid.nodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.maxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.add(startNode);

        while(openSet.count > 0)
        {
            Node currentNode = openSet.removeFirst();
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                retracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in grid.getNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMoveCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);

                if(newMoveCostToNeighbour < neighbour.gCost || !openSet.contains(neighbour))
                {
                    neighbour.gCost = newMoveCostToNeighbour;
                    neighbour.hCost = getDistance(neighbour, targetNode);

                    neighbour.parent = currentNode;

                    if(!openSet.contains(neighbour))
                    {
                        openSet.add(neighbour);
                    }
                    else
                    {
                        openSet.updateItem(neighbour);
                    }
                }
            }
        }
    }

    void retracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while(currentNode != start)
        {
            currentNode.worldPos = new Vector3(currentNode.worldPos.x, emp.transform.position.y, currentNode.worldPos.z); 
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        
        newPath = path;    
    }

    int getDistance(Node a, Node b)
    {
        float distX = a.gridX - b.gridX;
        float distY = a.gridY - b.gridY;

        return (int)Mathf.Sqrt((distX * distX) + (distY * distY));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(grid.gridWorldSize.x, 1, grid.gridWorldSize.y)); //Draws the grid

        if (grid != null)
        {
            foreach (Node n in grid.worldGrid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                if (newPath != null)
                {
                    if (newPath.Contains(n))
                    {
                        Gizmos.color = Color.blue;
                    }
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (grid.nodeDiameter - 0.1f));
            }
        }
    }
}