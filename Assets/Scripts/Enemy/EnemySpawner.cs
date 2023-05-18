using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

// This handles normal enemy spawn in the scene

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private FloatVariable timeSinceGameStart;
    
    private int dataIndex; // Incrementing this to add more EnemyData to the List of Spawnable Enemy
    [SerializeField] private float timeToIncrement;
    [SerializeField] private List<EnemySpawnData> spawnableEnemy;
    public EnemySpawnDataDistribution listOfSpawnableEnemy;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PickEnemyToSpawn(spawnableEnemy);
        }
    }


    private EnemySpawnData PickEnemyToSpawn(List<EnemySpawnData> listOfSpawnable)
    {
        foreach (EnemySpawnData data in listOfSpawnable)
        {
            listOfSpawnableEnemy.Add(data, data.weight);
        }

        EnemySpawnData returnData = listOfSpawnableEnemy.Draw();
        return returnData;
    }

}
