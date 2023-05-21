using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public EnemyData enemyData;

    // Data
    private float speed;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    [SerializeField] private Transform player;

    private Vector2 direction;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        if (enemyData != null)
        {
            LoadData(enemyData);
        }
    }

    private void FixedUpdate()
    {
        direction = ((Vector2) player.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void LoadData(EnemyData data)
    {
        enemyData = data;
        speed = data.speed;
        spriteRenderer.sprite = data.sprite;
    }
        
}
