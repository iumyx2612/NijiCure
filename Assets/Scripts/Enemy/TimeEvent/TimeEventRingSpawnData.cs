using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using MathHelper;

[CreateAssetMenu(menuName = "Enemy/Spawn Data/Ring Spawner")]
public class TimeEventRingSpawnData : TimeEventSpawnDataBase
{
    [Tooltip("Radius of the ring from the player")]
    public float distFromPlayer;

    protected override List<Vector2> SampleSpawnPosition()
    {
        List<Vector2> spawnPositions = new List<Vector2>();
        Vector2 direction = Vector2.right; // First direction
        float offsetAngle = 360f / spawnAmount;
        
        Vector2 firstPosition = PickFirstPosition(direction);
        spawnPositions.Add(firstPosition);
        for (int i = 1; i < spawnAmount; i++)
        {
            direction = AngleCal.DegreeToVector2(direction, offsetAngle);
            spawnPositions.Add(playerPosRef.Value + direction * distFromPlayer);
        }
        
        return spawnPositions;
    }

    private Vector2 PickFirstPosition(Vector2 direction)
    {
        Vector2 position = playerPosRef.Value + direction * distFromPlayer;
        return position;
    }
}
