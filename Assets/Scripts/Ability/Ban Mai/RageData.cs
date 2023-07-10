using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class RageData : PassiveAbilityBase
{
    public float damageIncrease;
    public Vector2 scaleIncrease;
    public IntGameEvent playerTakeDamage; // To Assign to Rage.cs

    public override void Initialize()
    {
        base.Initialize();
    }    
    public override void AddAndLoadComponent(GameObject player)
    {
        player.AddComponent<Rage>();
    }

    public override void TriggerAbility()
    {
        base.TriggerAbility();
    }

    public override void UpgradeAbility()
    {
        base.UpgradeAbility();
    }

    public override bool IsMaxLevel()
    {
        throw new System.NotImplementedException();
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        throw new System.NotImplementedException();
    }
}
