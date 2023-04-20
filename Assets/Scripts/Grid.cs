using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Vector2 gridWorldSize; // Area in world coord that grid is going to cover
    public float nodeRadius; // How much individual node covers
    private Node[,] grid; // Matrix of nodes
    
    // Variables to calculate how many nodes can fit in the grid area
    private float nodeDiameter; // Calculated diameter for ONE node
    private int numNodeX, numNodeY; // Number of nodes to fit in axis

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        numNodeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        numNodeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(
            gridWorldSize.x, gridWorldSize.y));
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
                // If there's 
                bool isObstacle = !(Physics2D.OverlapCircle(nodeCenter, nodeRadius));
            }
        }
    }
}
