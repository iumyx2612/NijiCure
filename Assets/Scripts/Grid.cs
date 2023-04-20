using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Vector2 gridWorldSize; // Area in world coord that grid is going to cover
    [SerializeField] private float nodeRadius; // How much individual node covers
    private Node[,] grid; // Matrix of nodes
    
    // Variables to calculate how many nodes can fit in the grid area
    private float nodeDiameter; // Calculated diameter for ONE node
    private int numNodeX, numNodeY; // Number of nodes to fit in axis

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        numNodeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        numNodeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        // Create grid 
        CreateGrid();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(
            gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                // If Node is obstacle, color with red, else white
                Gizmos.color = (node.isObstacle) ? Color.white : Color.red;
                Gizmos.DrawCube(node.nodeCenterPos, Vector3.one * (nodeDiameter - 0.05f));
            }
        }
    }

    // This function returns a node from world position
    private Node NodeFromWorldPos(Vector2 worldPos)
    {
        // We need to add (gridWorldSize / 2) because worldPos.x can be negative
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        
        // If for some fucking reasons, the percent is out of [0, 1] range
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        
        // Get the indices 
        int x = Mathf.RoundToInt((numNodeX - 1) * percentX);
        int y = Mathf.RoundToInt((numNodeY - 1) * percentY);
        return grid[x, y];
    }
    
    private void CreateGrid()
    {
        grid = new Node[numNodeX, numNodeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - 
                                  Vector2.right * gridWorldSize.x / 2 - 
                                  Vector2.up * gridWorldSize.y / 2;
        // Loop through all of the nodes to check if there's obstacle
        for (int x = 0; x < numNodeX; x++)
        {
            for (int y = 0; y < numNodeY; y++)
            {
                // Get the center point of the node in World coord
                Vector2 nodeCenter = worldBottomLeft +
                                     Vector2.right * (x * nodeDiameter + nodeRadius) +
                                     Vector2.up * (y * nodeDiameter + nodeRadius);
                // Check if that node is an obstacle
                // If there's collider means there's obstacle
                bool isObstacle = !Physics2D.OverlapCircle(nodeCenter, nodeRadius, obstacleMask);
                grid[x, y] = new Node(isObstacle, nodeCenter);
            }
        }
    }
}
