using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbilityBase : AbilityBase
{
    public override void Initialize()
    {
        state = AbilityState.active; // Make this always be active to remove from AbilityManager.cs
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
