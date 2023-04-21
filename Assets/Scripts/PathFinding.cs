using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PathFinding : MonoBehaviour
{
    private const int DIAG_COST = 14;
    private const int STRAIGHT_COST = 10;
    private Grid grid;

    [SerializeField] private Transform seeker;
    [SerializeField] private Transform target;

    private Node startNode;
    private Node targetNode;

    private Heap<Node> openSet;
    private HashSet<Node> closedSet;
    private List<Node> neighborNodes;

    
    private void Awake()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            FindPath(seeker.position, target.position);
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

    private void RetracePath(Node startNode, Node targetNode)
    {
        List<Node> paths = new List<Node>();
        Node currentNode = targetNode;
        // We now trace the path backward
        while (currentNode != startNode)
        {
            // Add currentNode to path
            paths.Add(currentNode);
            // Then trace to its parent
            currentNode = currentNode.parent;
        }
        // Currently path is in reversed order soooooooooo
        paths.Reverse();

        grid.paths = paths;
    }
    
    private void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        // Convert world position into Node
        startNode = grid.NodeFromWorldPos(startPos);
        targetNode = grid.NodeFromWorldPos(targetPos);
        
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
                sw.Stop();
                Debug.Log("Path Found: " + sw.ElapsedMilliseconds + "ms");
                // Retrace the path
                RetracePath(startNode, targetNode);
                return;
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
                }
                
            }
        }
    }

}
