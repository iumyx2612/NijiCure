using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageAbilityBase : AbilityBase
{
    [Header("Damage")]
    public int damage;
    [HideInInspector] public int currentDamage; // For in-game upgrade
    [HideInInspector] public float critChance; // Setup in AbilityManager.cs
    [HideInInspector] public float currentCritChance; 
    
    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;
        currentDamage = damage;
        currentCritChance = critChance;

        state = AbilityState.cooldown;
        isInitialized = true;
    }

    public override bool CanBeInit()
    {
        return true;
    }

    public override void TriggerAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void UpgradeAbility()
    {
        throw new System.NotImplementedException();
    }

    public abstract void ModifyDamage(float percentage, bool increase);
    
    public void BaseModifyDamage(float percentage, bool increase)
    {
        if (increase)
        {
            currentDamage = Mathf.RoundToInt( (1 + percentage) * currentDamage);
        }
        else
        {
            currentDamage = Mathf.RoundToInt(currentDamage / (1 + percentage));
        }
    }

    public void SetupCritChance(float value)
    {
        critChance = value;
    }

    public override void ModifyDebuff(int level) {}
}
