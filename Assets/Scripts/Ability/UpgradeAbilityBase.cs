using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAbilityBase : ScriptableObject
{
    [Serializable]
    public struct BaseUpgrades
    {
        public float percentCoolDownReduction;
    }
    
    [SerializeField]
    public BaseUpgrades baseUpgrades;
    
    // Really needs to come up with other solution!!!
    // Every inheritance must call to this FIRST BEFORE EXECUTING THEIR OWN UPGRADES
    public void ApplyUpgrade(AbilityBase abilityBase)
    {
        // Handle Cooldown
        float baseCooldown = abilityBase.baseCooldownTime; 
        float reducedCooldown = baseCooldown * baseUpgrades.percentCoolDownReduction/ 100;
        float newCooldown = baseCooldown - reducedCooldown;
        abilityBase.currentCooldownTime = newCooldown;
        // Other defined rules below
    }
}
