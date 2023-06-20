using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/Spawn Data/Spawn Data")]
public class SpawnData : ScriptableObject
{
    public float startTime;
    public float endTime;
    public int spawnAmount;
    public EnemyData enemyData;
    public int weight;
}
