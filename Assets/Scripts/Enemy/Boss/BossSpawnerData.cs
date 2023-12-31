using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Spawn Data/Mini Boss Spawner")]
public class BossSpawnerData : ScriptableObject
{
    [Header("Data of Boss to spawn")]
    public float speed;
    public int damage;
    public int health;
    public int expAmount;
    public GameEvent endStage;
    public EnemyAbilityBase enemyAbility;
    public RuntimeAnimatorController runtimeAnimatorController;

    [Header("Spawn Timing")]
    public float spawnTime;
    public Vector2Variable playerPosRef;
    public GameObjectCollection bossPool;
    private Vector2 baseSpawnArea = new Vector2(8, 7);
    private bool hasOccured;

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

    public void SpawnBoss(GameObject bossPrefab)
    {
        Vector2 spawnPos = PickSpawnPosition();
        bool canSpawn = false;
        for (int i = 0; i < bossPool.Count; i++)
        {
            GameObject bossHolder = bossPool[i];
            if (!bossHolder.activeSelf)
            {
                canSpawn = true;
                GameObject boss = bossHolder.transform.GetChild(0).gameObject;
                boss.GetComponent<BossMovement>().LoadData(this);
                boss.GetComponent<BossCombat>().LoadData(this);
                bossHolder.transform.position = spawnPos;
                bossHolder.SetActive(true);
                break;
            }
        }
        if (!canSpawn)
        {

        }
    }

    public Vector2 PickSpawnPosition()
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

    public bool HasOccured()
    {
        return hasOccured;
    }

    public void SetOccur(bool _bool)
    {
        hasOccured = _bool;
    }
}
