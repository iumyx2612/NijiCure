using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class DoAnHetHan : MonoBehaviour
{
    private DoAnHetHanData baseData;
    public DoAnHetHanData data;

    // Data
    [SerializeField] private LayerMask enemyMask;
    private CircleCollider2D selfCollider; // To assign counter 
    private MoveSpeedCounterData counterData;
    private int damage;
    private float critChance;
    private float cooldown;
    private float scale;
    private float baseRadius;
    private Vector2 baseScale; // Scale the GO with radius
    private float multiplier;
    
    // State
    private float internalCooldown;
    private bool canDamage;
    
    // Manage the Enemies
    private List<GameObject> affectedEnemies = new List<GameObject>();
    
    // Start is called before the first frame update
    private void Awake()
    {
        selfCollider = GetComponent<CircleCollider2D>();
        baseScale = transform.localScale;
        baseRadius = selfCollider.radius;
        if (data != null)
        {
            LoadData(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canDamage)
        {
            foreach (GameObject hitEnemy in affectedEnemies)
            {
                float randomNumber = Random.Range(0f, 1f);
                if (randomNumber < critChance)
                {
                    multiplier = 2f;
                }
                else
                {
                    multiplier = 1f;
                }
                hitEnemy.GetComponent<EnemyCombat>().TakeDamage(
                    damage, multiplier, Vector2.zero, 0f);
            }

            canDamage = false;
        }

        // State
        internalCooldown += Time.deltaTime;
        if (internalCooldown >= cooldown)
        {
            canDamage = true;
            internalCooldown = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyCounter script = collider.GetComponent<EnemyCounter>();
            script.AddMoveSpdCounter(counterData);
            affectedEnemies.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyCounter script = collider.GetComponent<EnemyCounter>();
            script.RemoveMoveSpdCounter(counterData);
            affectedEnemies.Remove(collider.gameObject);
        }
    }

    public void LoadData(DoAnHetHanData _data)
    {
        damage = _data.currentDamage;
        critChance = _data.currentCritChance;
        scale = _data.currentRadiusScale;
        if (scale != 1f)
        {
            transform.localScale = baseScale * scale;
        }
        cooldown = _data.currentCooldown;
        counterData = _data.counterData;
    }
}
