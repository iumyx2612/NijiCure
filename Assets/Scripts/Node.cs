using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isObstacle; // If the node is an obstacle
    public Vector3 nodeCenterPos; // Center of the Node in World Point coord

    public Node(bool _isObstacle, Vector3 _nodeCenterPos)
    {
        isObstacle = _isObstacle;
        nodeCenterPos = _nodeCenterPos;
    }
}
