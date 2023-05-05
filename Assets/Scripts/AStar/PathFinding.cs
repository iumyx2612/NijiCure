using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private const int DIAG_COST = 14;
    private const int STRAIGHT_COST = 10;
    private Grid grid;

    private Node startNode;
    private Node targetNode;

    private Heap<Node> openSet;
    private HashSet<Node> closedSet;
    private List<Node> neighborNodes;

    private PathRequestManager requestManager;

    public List<Vector2> wayPoints;

    
    private void Awake()
    {
        grid = gameObject.GetComponent<Grid>();
        requestManager = gameObject.GetComponent<PathRequestManager>();
    }
    
    private int CostBetweenTwoNodes(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return DIAG_COST * distY + STRAIGHT_COST * (distX - distY);
        }
        return DIAG_COST * distX + STRAIGHT_COST * (distY - distX);
    }

    // This function traverse the path from target Node back to start Node
    // And returns 
    private List<Vector2> RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        List<Vector2> waypoints = new List<Vector2>();
        Node currentNode = targetNode;
        // We now trace the path backward
        while (currentNode != startNode)
        {
            // Add currentNode to path
            path.Add(currentNode);
            // Then trace to its parent
            currentNode = currentNode.parent;
        }

        waypoints = PathToWorldCoord(path);
        
        // Currently path is in reversed order soooooooooo
        waypoints.Reverse();
        grid.path = path;

        return waypoints;
    }
    
    // This function takes each Node in path and 
    // turns into Array of world coord
    private List<Vector2> PathToWorldCoord(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
//            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,
//                path[i-1].gridX - path[i].gridY);
//            if (directionNew != directionOld)
//            {
//                waypoints.Add(path[i].nodeCenterPos);
//            }
//
//            directionOld = directionNew;
            waypoints.Add(path[i].nodeCenterPos);
        }

        return waypoints;
    }
    
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        wayPoints = new List<Vector2>();
        bool pathSuccess = false;
        
        // Convert world position into Node
        startNode = grid.NodeFromWorldPos(startPos);
        targetNode = grid.NodeFromWorldPos(targetPos);

        if (!targetNode.isObstacle)
        {
            // ---------------------- A* algorithm ----------------------
            // Create the openSet and the closedSet
            openSet = new Heap<Node>(grid.MaxSize);
            closedSet = new HashSet<Node>();
            
            // Add the start Node to the open Set
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // Find Node in the openSet with lowest F_cost
                Node currentNode = openSet.RemoveFirstItem();

                closedSet.Add(currentNode);
                
                // Found the Node
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }
                
                // Get all neighbors 
                neighborNodes = grid.GetNeighbors(currentNode);
                foreach (Node neighborNode in neighborNodes)
                {
                    // Check if neighbor is obstacle or in closedSet
                    if (neighborNode.isObstacle || closedSet.Contains(neighborNode))
                    {
                        continue;
                    }
                    // Check if new path to neighbor is shorter than old path
                    // OR neighbor not in openSet
                    int newCostToNeighbor = currentNode.gCost +
                                            CostBetweenTwoNodes(currentNode, neighborNode);
                    if (newCostToNeighbor < neighborNode.gCost || !openSet.Contains(neighborNode))
                    {
                        // Set F_cost of neighbor 
                        neighborNode.gCost = newCostToNeighbor;
                        neighborNode.hCost = CostBetweenTwoNodes(neighborNode, targetNode);
                        // Set parent of neighbor to currentNode
                        neighborNode.parent = currentNode;
                        // Check if neighbor not in the openSet
                        // add neighbor to openSet
                        if (!openSet.Contains(neighborNode))
                        {
                            openSet.Add(neighborNode);
                        }
                        else
                        {
                            openSet.UpdateItem(neighborNode);
                        }
                    }
                }
            }
        }
        
        yield return null; // Wait for one frame
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
        
    }

    // Will be used in PathRequestManager
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

}
