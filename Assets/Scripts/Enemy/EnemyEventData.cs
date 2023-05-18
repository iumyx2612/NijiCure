using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WhereToSpawn
{
    TopLeft, // Enemy moves from top left corner to PLAYER then to bottom right corner OF THE CAMERA
    TopRight, 
    BottomLeft,
    BottomRight,
    Top, // Enemy moves from top to PLAYER then to bottom OF THE CAMERA
    Right, // Enemy moves from right to PLAYER then to left OF THE CAMERA
    Bottom,
    Left,
    Circle, // This will spawn them in a circle surrounding the player
}

public enum SpawnStyle
{
    StaticCircle, // Enemy will make a circle around you and stay still
    
}


[CreateAssetMenu(menuName = "Enemy/Enemy Event")]
public class EnemyEventData : ScriptableObject
{
    public int timeToTrigger; // Time to trigger the event in SECONDS
    public int numToSpawn;
    public WhereToSpawn whereToSpawn;
    public EnemyData enemyData;
}
