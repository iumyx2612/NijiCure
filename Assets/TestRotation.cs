using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngleCalculation;


public class TestRotation : MonoBehaviour
{
    public Vector2 direction;
    public float angle;
    public Vector2 finalDirection;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, finalDirection);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction);
    }

    private void Update()
    {
        int mult = 1;
        if (direction.x < 0)
        {
            mult = -1;
        }
        finalDirection = AngleCal.DegreeToVector2(direction, angle * mult);
    }

    private void FixedUpdate()
    {
//        rb.MovePosition(rb.position + Vector2.right * Time.fixedDeltaTime * 5);
    }
}
