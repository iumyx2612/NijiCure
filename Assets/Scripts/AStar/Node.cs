using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isObstacle; // If the node is an obstacle
    public Vector3 nodeCenterPos; // Center of the Node in World Point coord
    
    // Keep track of where the Node is in the grid
    public int gridX; 
    public int gridY;
    
    public int gCost; // Cost from starting to end node
    public int hCost; // Cost from end to starting node

    public Node parent;

    private int heapIndex;

    public Node(bool _isObstacle, Vector3 _nodeCenterPos, int _gridX, int _gridY)
    {
        isObstacle = _isObstacle;
        nodeCenterPos = _nodeCenterPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
