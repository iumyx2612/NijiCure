using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountdownPassiveAbilityBase : AbilityBase
{
    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;
        state = AbilityState.cooldown;
    }
}
