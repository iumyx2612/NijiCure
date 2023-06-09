using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageAbilityBase : AbilityBase
{
    public int damage;
    public int currentDamage; // For in-game upgrade

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
}
