using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinhCovid : MonoBehaviour
{
    // Data
    private int damage;
    private float critChance;
    private float multiplier;
    private float timeToDamage; 
    private MoveSpeedCounter counter;
    private float radiusScale;
    private Vector2 baseScale;
    private Rigidbody2D rb;
    private CircleCollider2D selfCollider;
    private Vector2 newPosition;

    private List<Collider2D> affectedEnemies = new List<Collider2D>();

    // State management
    private bool canDamage;
    private float internalTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        selfCollider = GetComponent<CircleCollider2D>();
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        internalTime += Time.deltaTime;
        if (canDamage)
        {
            for (int i = 0; i < affectedEnemies.Count; i++)
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
                affectedEnemies[i].GetComponent<EnemyCombat>().TakeDamage(
                    damage, multiplier, Vector2.zero, 0f
                );
            }
            canDamage = false;
        }
        if (internalTime >= timeToDamage)
        {
            canDamage = true;
            internalTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            affectedEnemies.Add(collider);
            collider.GetComponent<EnemyCounter>().AddMoveSpdCounter(counter);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            affectedEnemies.Remove(collider);
            collider.GetComponent<EnemyCounter>().RemoveMoveSpdCounter(counter.counterName);
        }
    }

    public void SetNewPosition(Vector2 position)
    {
        newPosition = position;
    }

    public void LoadData(DinhCovidData data)
    {
        damage = data.currentDamage;
        critChance = data.critChance;
        timeToDamage = data.currentTimeToDamage;
        counter = data.counter;
        if (data.radiusScale > 1)
        {
            Vector2 newScale = baseScale * data.radiusScale;
            transform.localScale = newScale;
        }
    }
}
