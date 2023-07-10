using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;


public class Het : MonoBehaviour
{
    public HetData hetData;
    private HetData baseData;

    private Animator animator;
    private float animLength;
    private float internalLength;

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
    
    // For Ngong counters
    private Sprite counterSprite;
    private bool hasNgongPassiveAbility;
    private float counterPlaceChance;
    private float counterDuration;
    private float damageIncreasePerCounter;
    
    // Movement
    [SerializeField] private Vector2Variable playerPosRef; // Where to shoot ability from
    [SerializeField] private Vector2Variable directionRef; // What direction to shoot
    private Vector2 defaultScale;
    private Vector2 baseDirection; // Store direction to constantly update it during FixedUpdate
    [HideInInspector] public Vector2 oldBaseDirection; // To cache the baseDirection that x-axis diff from 0
    private Vector2 direction;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        defaultScale = transform.localScale;
        if (hetData != null)
        {
            LoadData(hetData);
        }
    }

    private void OnEnable()
    {
        animator.Play("Base Layer.BanMai_sonicwave");
        animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        baseDirection = new Vector2(directionRef.Value.x, 0);
        // Prevent new direction to have x-axis equals 0
        // If the x-axis equals 0 we simply take the oldBaseDirection
        // Why do we care abt x-axis equals 0?
        // If x-axis equals 0, the bullet will NOT MOVE
        if (baseDirection.x != 0)
            oldBaseDirection = baseDirection;
        transform.localScale = new Vector2(oldBaseDirection.x * defaultScale.x * scale.x,
            defaultScale.y * scale.y);
        transform.position = playerPosRef.Value + offset * oldBaseDirection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPosRef.Value + offset * oldBaseDirection;
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
        hasNgongPassiveAbility = true;
        counterSprite = _data.counterSprite;
        counterPlaceChance = _data.currentPlaceChance;
        counterDuration = _data.currentCounterDuration;
        damageIncreasePerCounter = _data.currentDamageIncrease;
    }
    
    private void ResetBullet()
    {
        internalLength = 0f;
        gameObject.SetActive(false);
        baseData.state = AbilityBase.AbilityState.cooldown; // The last deactivated bullet sets the state for the ability
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            // Only call to GetComponent
            EnemyCombat script = collider.GetComponent<EnemyCombat>();

            // For Ngong counters
            int numCounters = script.numCounters;
            
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
            script.TakeDamage(
                (int)(randomDamage + randomDamage * damageIncreasePerCounter * numCounters),
                multiplier,knockbackForce * direction, knockbackDuration);
            // Apply counter after dealing damage
            if (hasNgongPassiveAbility)
            {
                float counterRandom = Random.Range(0f, 1f);
                if (counterRandom <= counterPlaceChance)
                {
                    script.ModifyCounter(1, counterDuration, counterSprite);
                }
            }
        }
    }
}
