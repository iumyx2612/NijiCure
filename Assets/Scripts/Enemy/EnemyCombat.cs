using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public EnemyData enemyData;
    
    // Data
    private int damage;
    private int enemyHealth;
    private bool isAlive = true;
    
    private float attackRadius = 0.52f;
    private float damageTime = 1f;
    private float internalDamageTime;
    private bool canAttack = true;
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private IntGameEvent playerTakeDamage;
    [SerializeField] private ExpDropGameEvent dropExp;


    private void OnEnable()
    {
        if (enemyData != null)
        {
            LoadData(enemyData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        internalDamageTime += Time.deltaTime;
        if (internalDamageTime >= damageTime)
        {
            internalDamageTime = 0f;
            canAttack = true;
        }
    }

    private void FixedUpdate()
    {
        if (canAttack && isAlive)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position,
                attackRadius, playerMask);
            foreach (Collider2D player in hitPlayers)
            {
                playerTakeDamage.Raise(damage);
            }

            canAttack = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void TakeDamage(int damage) // Will be called in Bullet scripts
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            isAlive = false;
            dropExp.Raise(ConstructExpData());
            gameObject.SetActive(false);
        }
    }

    private ExpData ConstructExpData()
    {
        return new ExpData(enemyData.expDrop, transform.position);
    }

    public void LoadData(EnemyData data)
    {
        enemyData = data;
        damage = data.damage;
        enemyHealth = data.health;
    }
}
