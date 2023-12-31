using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;


public class Het : MonoBehaviour
{
    private HetData baseData;

    private Animator animator;
    private float animLength;
    private float internalLength;
    private bool isAwaken;

    // Data
    private Vector2 offset;
    private int damage;
    private float knockbackForce;
    private float knockbackDuration = 0.5f;
    private Vector2 scale;
    private float critChance;
    private float multiplier;
    // Debuff data
    private int bar;
    private int randomDamage;
    
    // For Ngong Ability
    private DamageBuffCounter ngongCounter;
    private bool hasNgongAbility;
    private float damageBuffPerCounter;
    private float counterPlaceChance;
    
    // Movement
    [SerializeField] private Vector2Variable playerPosRef; // Where to shoot ability from
    [SerializeField] private Vector2Variable directionRef; // What direction to shoot
    private Vector2 defaultScale;
    private Vector2 oldDirection; // To cache the baseDirection that x-axis diff from 0
    private Vector2 direction;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animLength = animator.runtimeAnimatorController.animationClips[0].length;
        defaultScale = transform.localScale;
        direction = Vector2.right;
    }

    private void OnEnable()
    {
        if (isAwaken)
        {
            animator.SetBool("isAwaken", true);
        }
        direction = new Vector2(directionRef.Value.x, 0);
        // Prevent new direction to have x-axis equals 0
        // If the x-axis equals 0 we simply take the oldBaseDirection
        // Why do we care abt x-axis equals 0?
        // If x-axis equals 0, the bullet will NOT MOVE
        if (direction.x != 0)
            oldDirection = direction;
        transform.localScale = new Vector2(oldDirection.x * defaultScale.x * scale.x,
            defaultScale.y * scale.y);
        transform.position = playerPosRef.Value + offset * oldDirection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPosRef.Value + offset * oldDirection;
        internalLength += Time.deltaTime;
        if (internalLength >= animLength)
        {
            ResetBullet();
        }
    }

    public void LoadData(HetData _data)
    {
        if (baseData == null)
        {
            baseData = _data;
        }

        offset = _data.offset;
        damage = _data.currentDamage;
        knockbackForce = _data.currentKnockbackForce;
        scale = _data.currentScale;
        bar = _data.bar;
    }

    public void LoadNgongData(NgongData _data)
    {
        hasNgongAbility = true;
        counterPlaceChance = _data.currentPlaceChance;
        ngongCounter = _data.currentCounter;
        damageBuffPerCounter = ngongCounter.damageBuff;
    }
    
    private void ResetBullet()
    {
        internalLength = 0f;
        gameObject.SetActive(false);
        baseData.state = AbilityBase.AbilityState.cooldown; // The last deactivated bullet sets the state for the ability
    }

    public void Awaken()
    {
        animLength = animator.runtimeAnimatorController.animationClips[1].length + 
        animator.runtimeAnimatorController.animationClips[0].length;
        isAwaken = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            // Only call to GetComponent
            EnemyCombat combatScript = collider.GetComponent<EnemyCombat>();
            EnemyCounter counterScript = collider.GetComponent<EnemyCounter>();

            // For Ngong counters
            int numCounters = 0;
            if (hasNgongAbility)
            {
                numCounters = counterScript.GetNumDmgBuffCounter(ngongCounter);
            }
            
            // For crit
            float critRandom = Random.Range(0f, 1f);
            if (critRandom <= critChance)
            {
                multiplier = 1.5f;
            }
            else
            {
                multiplier = 1f;
            }

            randomDamage = Random.Range(damage - bar, damage + bar);
            combatScript.TakeDamage(
                Mathf.RoundToInt(randomDamage + randomDamage * damageBuffPerCounter * numCounters),
                multiplier, knockbackForce * direction, knockbackDuration);
            // Apply counter after dealing damage
            if (hasNgongAbility)
            {
                float counterRandom = Random.Range(0f, 1f);
                if (counterRandom <= counterPlaceChance)
                {
                    counterScript.AddDmgBuffCounter(ngongCounter);
                    ngongCounter.Active(collider.transform.position);
                }
            }
        }
    }
}
