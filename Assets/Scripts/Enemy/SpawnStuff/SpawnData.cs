using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/Spawn Data/Spawn Data")]
public class SpawnData : ScriptableObject
{
    [Tooltip("Time to start spawning Enemy")]
    public float startTime;
    [Tooltip("Time to stop spawning Enemy, ignore if Event")]
    public float endTime;
    [Tooltip("Number of enemies to spawn each period")]
    public int spawnAmount;
    [Tooltip("Data of Enemy to spawn, ignore if Event")]
    public EnemyData enemyData;
    [Tooltip("Weight to get picked in a distribution, ignore if Event")]
    public int weight;
}
