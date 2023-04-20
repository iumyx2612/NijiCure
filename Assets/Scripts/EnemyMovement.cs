using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private Rigidbody2D rb2D;
    private Vector2 movement;

    [SerializeField] private IntGameEvent playerTakeDamage;
    [SerializeField] private IntVariable damage;
    
    // Update is called once per frame
    private void Update()
    {
        // Handle inputs
        movement.x = -Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        // Handle physics
        rb2D.MovePosition(rb2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerTakeDamage.Raise(damage);
        }
    }
}
