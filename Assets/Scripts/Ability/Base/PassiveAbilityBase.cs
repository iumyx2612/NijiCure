using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbilityBase : AbilityBase
{
    public override void Initialize()
    {
    }

    public override void TriggerAbility()
    {
    }

    public override void UpgradeAbility()
    {
        throw new System.NotImplementedException();
    }

    public abstract void AddAndLoadComponent(GameObject objectToAdd);
}
