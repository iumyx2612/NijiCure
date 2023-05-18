using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Spawn Data")]
public class EnemySpawnData : ScriptableObject
{
    public EnemyData enemyData;
    public int weight;
    public int minToSpawn;
    public int maxToSpawn;
    public float timeToSpawn;
    public float timeToDespawn;
}
