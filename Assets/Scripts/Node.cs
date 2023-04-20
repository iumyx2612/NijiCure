using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isObstacle;
    public Vector3 worldPos;

    public Node(bool _isObstacle, Vector3 _worldPos)
    {
        isObstacle = _isObstacle;
        worldPos = _worldPos;
    }
}
