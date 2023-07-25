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

    public TimeEventEnemyData timeEventEnemyData;
    public GameObjectCollection timeEventEnemyPool;
    private bool hasOccured; 
    // Pass to TimeEventEnemyData
    public float lifeTime;
    public bool oneTime;
    public Vector2Reference destination; // if oneTime is set then this is ignored

    public void SetRequiresDataField()
    {
        timeEventEnemyData.lifeTime = lifeTime;
        timeEventEnemyData.oneTime = oneTime;
    }

    public abstract void SpawnTimeEventEnemy(GameObject enemyPrefab); // Called by EnemySpawner.cs

    public bool HasOccured()
    {
        return hasOccured;
    }

    public void SetOccur(bool _bool)
    {
        hasOccured = _bool;
    }
}
