using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Spawn Data/Wall Spawner")]
public class TimeEventWallSpawnData : TimeEventSpawnDataBase
{
    /// <summary>
    /// Already has:
    ///     public float starTime;
    ///     public float endTime;
    ///     public int spawnAmount;
    ///     public EnemyData enemyData; (not use)
    ///     public int weight; (not use)
    ///     public TimeEventEnemyData timeEventEnemyData;
    ///     public GameObjectCollection timeEventEnemyPool;
    ///     public float lifeTime;
    ///     public bool oneTime;
    /// </summary>
    public Vector2Variable playerPosRef;
    public Vector2 firstEnemyDist;
    public Vector2 spacingPerEnemy;


    protected override List<Vector2> SampleSpawnPosition()
    {
        List<Vector2> spawnPositions = new List<Vector2>();
        Vector2 basePos = PickBasePosition();
        spawnPositions.Add(basePos);
        // Spawn half of the wall
        for (int i = 0; i < spawnAmount / 2; i++)
        {
            Vector2 pos = basePos + spacingPerEnemy * (i + 1);
            spawnPositions.Add(pos);
        }
        // Spawn the other half
        for (int i = 0; i < spawnAmount / 2; i++)
        {
            Vector2 pos = basePos - spacingPerEnemy * (i + 1);
            spawnPositions.Add(pos);
        }

        return spawnPositions;
    }
    
    private Vector2 PickBasePosition()
    {
        return playerPosRef.Value + firstEnemyDist;
    }
}
