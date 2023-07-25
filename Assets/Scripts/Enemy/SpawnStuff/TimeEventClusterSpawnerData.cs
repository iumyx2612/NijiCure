using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/Spawn Data/Cluster Spawner")]
public class TimeEventClusterSpawnerData : TimeEventSpawnDataBase
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
    public float radius; // How big the square
    public Vector2Variable playerPosRef;
    public Vector2 baseSpawnArea;

    public override void SpawnTimeEventEnemy(GameObject enemyPrefab)
    {
        timeEventEnemyData.destination = destination.Value;
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
                    timeEventEnemyHolder.transform.position = spawnPositions[i];
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
