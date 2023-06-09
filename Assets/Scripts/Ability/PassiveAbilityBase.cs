using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbilityBase : AbilityBase
{
    public override void Initialize()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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
