using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class SummonAbility : MonoBehaviour
{
    public SummonAbilityData summonData;
    
    // Data
    private int damage;
    private float summonRadius;
    private float summonSpeed;
    private float timePerAttack;
    private float summonAttackRadius;
    private SpriteRenderer spriteRenderer;
    
    // Runtime data
    private float internalTime;
    
    // Movement related
    private Rigidbody2D rb;
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private LayerMask enemyMask;
    public Vector2 nextPos; // The destination
    private float epsilon = 0.2f; // The smallest diff to check if summon reached its position
    
    
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (summonData != null)
        {
            LoadBulletData(summonData);
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        transform.position = playerPosRef.Value;
    }

    // Update is called once per frame
    void Update()
    {
        // If state is attacking
        if (summonData.summonState == SummonAbilityData.SummonState.attacking)
        {
            Debug.Log("Attack");
            // Perform a single attack
            Attack();
            // The switch to cooldown state
            summonData.summonState = SummonAbilityData.SummonState.cooldown;
        }

        if (summonData.summonState == SummonAbilityData.SummonState.cooldown)
        {
            Debug.Log("Cooldown");
            // Cooldown 
            internalTime += Time.deltaTime;
            if (internalTime >= timePerAttack)
            {
                // Change to move state at the end of cooldown
                // Then pick a new Position to move the summon
                internalTime = 0f;
                summonData.summonState = SummonAbilityData.SummonState.moving;
                nextPos = PickNextPosition(playerPosRef.Value, summonRadius);
            }
        }
    }

    private void FixedUpdate()
    {
        // If the state is moving, then we move the summon
        if (summonData.summonState == SummonAbilityData.SummonState.moving)
        {
            Debug.Log("Moving");
            Vector2 direction = (nextPos - rb.position).normalized;
            rb.MovePosition(rb.position + direction * summonSpeed * Time.fixedDeltaTime);
            if (ReachedDest(rb.position, nextPos))
            {
                Debug.Log("Reach dest");
                // If the summon reached the Position, switch to attack state
                summonData.summonState = SummonAbilityData.SummonState.attacking;
            }
        }
    }

    public void LoadBulletData(SummonAbilityData data)
    {
        summonData = data;
        damage = data.damage;
        summonRadius = data.summonRadius;
        summonSpeed = data.summonSpeed;
        timePerAttack = data.timePerAttack;
        summonAttackRadius = data.summonAttackRadius;
        spriteRenderer.sprite = data.sprite;
    }
    
    // This function picks a random location, given center position and radius
    private Vector2 PickNextPosition(Vector2 center, float radius)
    {
        float center_x = center.x;
        float center_y = center.y;
        float min_x = center_x - radius;
        float max_x = center_x + radius;
        float min_y = center_y - radius;
        float max_y = center_y + radius;
        float pos_x = Random.Range(min_x, max_x);
        float pos_y = Random.Range(min_y, max_y);
        Vector2 newPos = new Vector2(pos_x, pos_y);
        return newPos;
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,
            summonAttackRadius, enemyMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }  
    }

    // This function check if posA is close to posB
    // return true if yes
    private bool ReachedDest(Vector2 posA, Vector2 posB)
    {
        if (Vector2.Distance(posA, posB) <= epsilon)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
            Gizmos.DrawWireSphere(rb.position, summonAttackRadius);
    }
}
