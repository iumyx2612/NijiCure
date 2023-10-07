using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public abstract class TimeEventSpawnDataBase : ScriptableObject
{
    [Tooltip("When to spawn the Event")]
    public float spawnTime;

    [Tooltip("Amount to spawn")]
    public int spawnAmount;

    [Header("Data of spawn Enemies")]
    public float speed;
    public int damage;
    public int health;
    public int expAmount;
    public RuntimeAnimatorController runtimeAnimatorController;
    
    [Tooltip("How long the event Enemy last")]
    public float lifeTime;

    [Tooltip("If false, Enemy follows player")]
    public bool oneTime;

    [Tooltip("The destination to move to when One Time is set")]
    public Vector2 destination;

    [Tooltip("Play sound when event happens")]
    public Sound soundEvent;

    public enum Shape
    {
        horizontal,
        vertical,
        square
    }

    public Shape shape;
    [HideInInspector] public Dictionary<Shape, (Vector2, Vector2)> shapeToColliderMapping = 
        new Dictionary<Shape, (Vector2, Vector2)>
        {
            {Shape.horizontal, (new Vector2(0.33f, 0.2f), new Vector2(0, -0.06f))},
            {Shape.vertical, (new Vector2(0.24f, 0.13f), new Vector2(0, -0.08f))},
            {Shape.square, (new Vector2(0.24f, 0.22f), new Vector2(0, -0.08f))}
        };
    public GameObjectCollection timeEventEnemyPool;
    private bool hasOccured;

    public Vector2Variable playerPosRef;

    public void SpawnTimeEventEnemy(GameObject enemyPrefab) // Called by EnemySpawner.cs
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
                    timeEventEnemy.GetComponent<TimeEventEnemyMovement>().LoadData(this, spawnPositions[numRequired]);
                    timeEventEnemy.GetComponent<TimeEventEnemyCombat>().LoadData(this);
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
                GameObject timeEventEnemy = enemyHolder.transform.GetChild(0).gameObject;
                timeEventEnemy.GetComponent<TimeEventEnemyMovement>().LoadData(this, spawnPositions[i]);
                timeEventEnemy.GetComponent<TimeEventEnemyCombat>().LoadData(this);
                enemyHolder.transform.position = spawnPositions[i];
                timeEventEnemyPool.Add(enemyHolder);
            }
        }

        // Play Sound
        AudioManager.Instance.Play(soundEvent.audioName);
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
