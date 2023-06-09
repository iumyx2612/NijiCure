using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

// This handles normal enemy spawn in the scene

public class EnemySpawner : MonoBehaviour
{
    private EnemySpawnDataDistribution spawnDistribution;
    
    // Configurable in the editor
    [SerializeField] private GameObjectCollection enemyHolderPool;
    [SerializeField] private FloatVariable timeSinceGameStart;
    [SerializeField] private GameObject enemyHolderPrefab;
    [SerializeField] private List<SpawnData> stageSpawnData; // The Spawn Data of the entire stage
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private Vector2 baseSpawnArea;
    
    private int index = 0;
    private List<SpawnData> currentSpawnData = new List<SpawnData>(); // The Spawn Data in the current moment of the game

    private float internalSpawnCooldown;
    [SerializeField] private int spawnCooldown; // how fast to spawn enemy
    [SerializeField] private int maxEnemyLimit; // For performance purpose and easy mode


    private void Awake()
    {
        enemyHolderPool.Clear();
        timeSinceGameStart.Value = 0f;
        spawnDistribution = gameObject.GetComponent<EnemySpawnDataDistribution>();
    }

    private void Start()
    {
        InitializeEnemy();
        currentSpawnData.Add(stageSpawnData[0]); // Start the game with the first Spawn Data
        spawnDistribution.Add(stageSpawnData[0], stageSpawnData[0].weight);
    }

    private void Update()
    {
        timeSinceGameStart.Value += Time.deltaTime;
        // At the last element of list
        if (index < stageSpawnData.Count - 1)
        {
            // Check for next SpawnData.starTime, if meet requirement, add to currentSpawnData 
            if (stageSpawnData[index + 1].starTime <= timeSinceGameStart.Value)
            {
                currentSpawnData.Add(stageSpawnData[index + 1]);
                spawnDistribution.Add(stageSpawnData[index + 1], stageSpawnData[index + 1].weight);
                index += 1;
            }
        }
        // Check for SpawnData.endTime, if meet requirement, remove from currentSpawnData
        // Reverse order since we have to remove things from List 
        for (int i = currentSpawnData.Count - 1; i >= 0; i--)
        {
            if (currentSpawnData[i].endTime < timeSinceGameStart.Value)
            {
                currentSpawnData.RemoveAt(i);
                spawnDistribution.RemoveAt(i);
            }
        }
        
        // Spawn enemy
        internalSpawnCooldown += Time.deltaTime;
        if (internalSpawnCooldown >= spawnCooldown)
        {
            SpawnEnemy();
            internalSpawnCooldown = 0f;
        }

    }
    
    private void InitializeEnemy()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject enemyHolder = Instantiate(enemyHolderPrefab, transform);
            enemyHolderPool.Add(enemyHolder);
            enemyHolder.SetActive(false);
        }
    }
    
    private void SpawnEnemy()
    {
        SpawnData spawnData = spawnDistribution.Draw();
        // Check for number of inactive Enemy Prefab
        int numInActive = 0;
        foreach (GameObject enemyHolder in enemyHolderPool)
        {
            if (!enemyHolder.activeSelf)
            {
                numInActive += 1;
                if (numInActive >= spawnData.spawnAmount)
                {
                    break;
                }
            }
        }
        
        int numRequired = 0;
        // If numInActive is large enough to support Instantiate from Pool
        if (numInActive >= spawnData.spawnAmount)
        {
            // Grab Prefab from Pool to active
            for (int i = 0; i < enemyHolderPool.Count; i++)
            {
                GameObject enemyHolder = enemyHolderPool[i];
                if (!enemyHolder.activeSelf)
                {
                    GameObject enemy = enemyHolder.transform.GetChild(0).gameObject;
                    enemy.GetComponent<EnemyMovement>().LoadData(spawnData.enemyData);
                    enemy.GetComponent<EnemyCombat>().LoadData(spawnData.enemyData);
                    enemyHolder.transform.position = PickSpawnPosition();
                    enemyHolder.SetActive(true);
                    numRequired += 1;
                }

                if (numRequired >= spawnData.spawnAmount)
                {
                    break;
                }
            }   
        }
        else
        {
            for (int i = 0; i < spawnData.spawnAmount; i++)
            {
                GameObject enemyHolder = Instantiate(enemyHolderPrefab, transform);
                GameObject enemy = enemyHolder.transform.GetChild(0).gameObject;
                enemy.GetComponent<EnemyMovement>().LoadData(spawnData.enemyData);
                enemy.GetComponent<EnemyCombat>().LoadData(spawnData.enemyData);
                enemyHolder.transform.position = PickSpawnPosition();
            }
        }
    }

    private Vector2 PickSpawnPosition()
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
