using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class DoAnHetHan : MonoBehaviour
{
    // Data
    private CircleCollider2D selfCollider; // To assign counter 
    private MoveSpeedCounter counter;
    private int damage;
    private float cooldown;
    private float scale;
    private Vector2 baseScale; // Scale the GO with radius
    
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
    }

    // Update is called once per frame
    void Update()
    {
        if (canDamage)
        {
            foreach (GameObject hitEnemy in affectedEnemies)
            {
                hitEnemy.GetComponent<EnemyCombat>().TakeDamage(
                    damage, 1f, Vector2.zero, 0f);
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
            script.AddMoveSpdCounter(counter);
            affectedEnemies.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyCounter script = collider.GetComponent<EnemyCounter>();
            script.RemoveMoveSpdCounter(counter.counterName);
            affectedEnemies.Remove(collider.gameObject);
        }
    }

    public void LoadData(DoAnHetHanData _data)
    {
        damage = _data.currentDamage;
        scale = _data.currentRadiusScale;
        if (scale != 1f)
        {
            transform.localScale = baseScale * scale;
        }
        cooldown = _data.currentCooldownTime;
        counter = _data.currentCounter;
    }
}
