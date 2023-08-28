using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngleCalculation;


public class TestRotation : MonoBehaviour
{
    public Vector2 firstPoint;
    public Vector2 center;
    private Vector2 direction;
    public List<Vector2> points = new List<Vector2>();
    public float dist;
    public float offsetAngle;
    public int numPoints;

    private void Start()
    {
        direction = Vector2.right;
        firstPoint = center + direction * dist;
        points.Add(firstPoint);
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(points[i], 0.1f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("A");
            points.Clear();
            offsetAngle = 360f / numPoints;
            for (int i = 1; i < numPoints; i++)
            {
                direction = AngleCal.DegreeToVector2(direction, offsetAngle);
                points.Add(center + direction * dist);
            }
        }
    }
}
