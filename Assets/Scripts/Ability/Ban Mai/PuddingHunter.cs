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
    private Sprite abilityIcon;
    private PassiveAbilityGameEvent activeCountdownImage; // Setup in UIManager.cs
    
    private void Awake()
    {
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
        // UI
        activeCountdownImage.Raise(new PassiveAbilityInfo(buffTime, abilityIcon, 0, false, false));
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
            script.RemoveItemDropCounter(counter.counterName);
        }
    }

    public void LoadData(PuddingHunterData _data)
    {
        radius = _data.currentRadius;
        selfCollider.radius = radius;
        counter = _data.currentCounter;
        buffTime = _data.currentBuffTime;
        damageIncrease = _data.currentDamageIncrease;
        if (baseHetAbility == null)
            baseHetAbility = _data.baseHetAbility;
        if (puddingHunterGameEvent == null)
        {
            puddingHunterGameEvent = _data.puddingHunterGameEvent;
            puddingHunterGameEvent.AddListener(TriggerBuff);   
        }

        if (activeCountdownImage == null)
        {
            abilityIcon = _data.abilityIcon;
            activeCountdownImage = _data.activeCountdownImage;
        }
    }
}
