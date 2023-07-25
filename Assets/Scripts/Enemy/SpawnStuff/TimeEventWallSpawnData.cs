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

    public override void SpawnTimeEventEnemy(GameObject enemyPrefab)
    {
        List<Vector2> spawnPositions = SampleSpawnPosition();
        
        // Check for number of inactive Enemy Prefab
        int numInActive = 0;
        foreach (GameObject enemyHolder in timeEventEnemyPool)
        {
            if (!enemyHolder.activeSelf)
            {
                numInActive += 1;
                if (numInActive >= spawnAmount)
                {
                    break;
                }
            }
        }
        int numRequired = 0;
        // If numInActive is large enough to support Instantiate from Pool
        if (numInActive >= spawnAmount)
        {
            // Grab Prefab from Pool to active
            for (int i = 0; i < timeEventEnemyPool.Count; i++)
            {
                GameObject timeEventEnemyHolder = timeEventEnemyPool[i];
                if (!timeEventEnemyHolder.activeSelf)
                {
                    GameObject timeEventEnemy = timeEventEnemyHolder.transform.GetChild(0).gameObject;
                    timeEventEnemy.GetComponent<TimeEventEnemyMovement>().LoadData(timeEventEnemyData);
                    timeEventEnemy.GetComponent<EnemyCombat>().LoadData(timeEventEnemyData);
                    timeEventEnemyHolder.transform.position = spawnPositions[numRequired];
                    timeEventEnemyHolder.SetActive(true);
                    numRequired += 1;
                    if (numRequired >= spawnAmount)
                    {
                        break;
                    }
                }
            }   
        }
        else
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                GameObject holder = GameObject.Find("Time Event Enemy Holder");
                GameObject enemyHolder = Instantiate(enemyPrefab, holder.transform);
                GameObject enemy = enemyHolder.transform.GetChild(0).gameObject;
                enemy.GetComponent<TimeEventEnemyMovement>().LoadData(timeEventEnemyData);
                enemy.GetComponent<EnemyCombat>().LoadData(timeEventEnemyData);
                enemyHolder.transform.position = spawnPositions[i];
                timeEventEnemyPool.Add(enemyHolder);
            }
        }
    }

    private List<Vector2> SampleSpawnPosition()
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
