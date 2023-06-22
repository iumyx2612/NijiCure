using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageAbilityBase : AbilityBase
{
    public int damage;
    [HideInInspector] public int currentDamage; // For in-game upgrade
    public float critChance; // Setup in AbilityManager.cs
    public float currentCritChance; 
    
    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void UpgradeAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void PartialModify(int value){}

    public void SetupCritChance(float value)
    {
        critChance = value;
    }
}
