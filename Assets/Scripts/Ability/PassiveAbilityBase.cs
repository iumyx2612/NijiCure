using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbilityBase : AbilityBase
{
    // The Passive Ability Script will be attached directly to Player
    public GameObject player;
    
    public override void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AddAndLoadComponent(player);
    }

    public override void TriggerAbility()
    {
    }

    public override void UpgradeAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void PartialModify(int value)
    {
        throw new System.NotImplementedException();
    }

    public abstract void AddAndLoadComponent(GameObject player);
}
