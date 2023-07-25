using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class PuddingHunter : MonoBehaviour
{
    private CircleCollider2D selfCollider;
    
    // Data
    private SpriteRenderer spriteRenderer; // To display the area of effect
    private GameEvent puddingHunterGameEvent; // Raise in BanMaiPudding.cs
    private float radius;
    private float damageIncrease;
    private ItemDropCounter counter;
    private float buffTime;
    private float internalBuffTime;
    private HetData baseHetAbility; // We take the Base Ability of Het
    
    // Act as state management system
    private bool havingBuff;
    
    // UI
    private PassiveAbilityGameEvent activeCountdownImage; // Setup in UIManager.cs
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selfCollider = GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
    }

    private void OnDisable()
    {
        puddingHunterGameEvent.RemoveListener(TriggerBuff);
    }

    // State manager
    private void Update()
    {
        if (havingBuff)
        {
            internalBuffTime += Time.deltaTime;
            if (internalBuffTime >= buffTime)
            {
                havingBuff = false;
                baseHetAbility.ModifyDamage(damageIncrease, false);
            }
        }
    }

    private void TriggerBuff()
    {
        // Reset buff time
        internalBuffTime = 0f;

        if (!havingBuff)
        {
            havingBuff = true;
            baseHetAbility.ModifyDamage(damageIncrease, true);
        }
    }

    // Place ONE ItemDropCounter onto an Enemy
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyCounter script = collider.GetComponent<EnemyCounter>();
            script.AddItemDropCounter(counter);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyCounter script = collider.GetComponent<EnemyCounter>();
            script.RemoveItemDropCounter(counter);
        }
    }

    public void LoadData(PuddingHunterData _data)
    {
        radius = _data.radius;
        selfCollider.radius = radius;
        counter = _data.itemDropCounter;
        buffTime = _data.buffTime;
        damageIncrease = _data.damageIncrease;
        if (baseHetAbility == null)
            baseHetAbility = _data.baseHetAbility;
        if (puddingHunterGameEvent == null)
        {
            puddingHunterGameEvent = _data.puddingHunterGameEvent;
            puddingHunterGameEvent.AddListener(TriggerBuff);   
        }
    }
}
