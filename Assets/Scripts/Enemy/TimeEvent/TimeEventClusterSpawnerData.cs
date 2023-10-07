using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/Spawn Data/Cluster Spawner")]
public class TimeEventClusterSpawnerData : TimeEventSpawnDataBase
{    
    [Header("Spawn position")]
    [Tooltip("Enemies will spawn inside a circle of this radius")]
    public float radius; 
    [Tooltip("How far the center of spawn circle from player")]
    public Vector2 baseSpawnArea;
    
    protected override List<Vector2> SampleSpawnPosition()
    {
        List<Vector2> spawnPositions = new List<Vector2>();
        Vector2 spawnPos = PickBasePosition();
        Vector2 topLeft = new Vector2(spawnPos.x - radius, spawnPos.y + radius);
        Vector2 botRight = new Vector2(spawnPos.x  + radius, spawnPos.y - radius);
        for (int i = 0; i < spawnAmount; i++)
        {
            // Sample a random position in the spawn square
            Vector2 position = new Vector2(Random.Range(topLeft.x, botRight.x),
                Random.Range(topLeft.y, botRight.y));
            spawnPositions.Add(position);
        }

        return spawnPositions;
    }
    
    private Vector2 PickBasePosition()
    {
        Vector2 spawnPos = new Vector2();
        float randomNumber = Random.value;
        if (randomNumber > 0.5f)
        {
            randomNumber = 1f;
        }
        else
        {
            randomNumber = -1f;
        }
        
        // Spawn randomly on the X-axis
        if (Random.value > 0.5f)
        {
            spawnPos.x = Random.Range(-baseSpawnArea.x, baseSpawnArea.x);
            spawnPos.y = baseSpawnArea.y * randomNumber; // The Y-axis will be one of two terminal
        }
        // Spawn randomly on the Y-axis
        else
        {
            spawnPos.x = baseSpawnArea.x * randomNumber;
            spawnPos.y = Random.Range(-baseSpawnArea.y, baseSpawnArea.y);
        }
        
        // Add it with the player player position -> Spawn enemy will always be around player
        spawnPos += playerPosRef.Value;

        return spawnPos;
    } 
}
