using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MathHelper
{
    public struct AngleCal
    {
        public static Vector2 DegreeToVector2(Vector2 _baseDirection, float degree)
        {
            float radian = degree * Mathf.Deg2Rad;
            return new Vector2(_baseDirection.x * Mathf.Cos(radian) - _baseDirection.y * Mathf.Sin(radian),
                _baseDirection.x * Mathf.Sin(radian) + _baseDirection.y * Mathf.Cos(radian));
        }
        
        public static float Vector2ToDegree(Vector2 _baseDirection, Vector2 referenceDirection)
        {
            return Vector2.Angle(referenceDirection, _baseDirection);
        } 
    }

    public struct PositionSampling
    {
        public static Vector2 RandomPositionInSquare(Vector2 middlePoint, Vector2 square)
        {
            float maxXDist = middlePoint.x + square.x;
            float minXDist = middlePoint.x - square.x;
            float maxYDist = middlePoint.y + square.y;
            float minYDist = middlePoint.y - square.y;

            float xPos = Random.Range(minXDist, maxXDist);
            float yPos = Random.Range(minYDist, maxYDist);
            
            return new Vector2(xPos, yPos);
        }
    }

}
