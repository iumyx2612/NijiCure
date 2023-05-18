using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

// This script contains enemy behavior EXCEPT FOR MOVEMENT
public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    [SerializeField] private Transform player;
    private Rigidbody2D rb;
    
    // Data
    private float speed;
    private int damage;
    private int enemyHealth;
    private List<float> expDropChances;
    private SpriteRenderer spriteRenderer;
    
    private int currentEnemyHealth;
    private Vector2 direction;
    private bool isAlive;
    
    [SerializeField] private ExpDropGameEvent chooseExpGameEvent;
    

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (enemyData != null)
        {
            LoadData(enemyData);
        }
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            direction = ((Vector2) player.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    private void OnEnable()
    {
        currentEnemyHealth = enemyHealth;
        isAlive = true;
    }

    public void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
        if (currentEnemyHealth <= 0)
        {
            OnEnemyDie();
        }
    }
    
    // This triggers when enemy dies
    private void OnEnemyDie()
    {
        isAlive = false;
        ExpDrop expDrop = ConstructExpDrop();
        chooseExpGameEvent.Raise(expDrop);
        gameObject.SetActive(false);
    }

    private void LoadData(EnemyData data)
    {
        speed = data.speed;
        damage = data.damage;
        enemyData = data;
        enemyHealth = data.health;
        expDropChances = data.expDropChances;
        spriteRenderer.sprite = data.sprite;
    }

    private ExpDrop ConstructExpDrop()
    {
        ExpDrop expDrop = new ExpDrop();
        expDrop.position = transform.position;
        expDrop.chances = expDropChances;
        expDrop.level = 0; // This will be changed in LevelingSystem.cs
        return expDrop;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
