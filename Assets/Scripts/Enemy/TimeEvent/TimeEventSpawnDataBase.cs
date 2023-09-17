using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public abstract class TimeEventSpawnDataBase : SpawnData
{
    /// <summary>
    /// Already has:
    ///    public float starTime;
    ///    public float endTime;
    ///    public int spawnAmount;
    ///    public EnemyData enemyData; (not used)
    ///    public int weight; (not used)
    /// </summary>

    [Header("Where to parse the data to")]
    public TimeEventEnemyData timeEventEnemyData;
    public GameObjectCollection timeEventEnemyPool;
    private bool hasOccured; 
    [Header("Data to parse to Time Event Enemy Data")]
    [Tooltip("How long the event Enemy last")]
    public float lifeTime;
    [Tooltip("If false, Enemy follows player")]
    public bool oneTime;
    public Vector2Variable playerPosRef;
    public Vector2 destination;

    public void SetRequiresDataField() // Called by EnemySpawner.cs
    {
        timeEventEnemyData.lifeTime = lifeTime;
        timeEventEnemyData.oneTime = oneTime;
    }

    public void SpawnTimeEventEnemy(GameObject enemyPrefab) // Called by EnemySpawner.cs
    {
        timeEventEnemyData.destination = playerPosRef.Value + destination;
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

    protected abstract List<Vector2> SampleSpawnPosition();

    public bool HasOccured()
    {
        return hasOccured;
    }

    public void SetOccur(bool _bool)
    {
        hasOccured = _bool;
    }
}
