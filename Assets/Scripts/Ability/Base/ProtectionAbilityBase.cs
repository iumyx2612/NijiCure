using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProtectionAbilityBase<T> : AbilityBase
{
    public enum ProtectionType
    {
        dodge,
        shield,
        heal,
        reduce
    }
    [Header("Protection Base")]
    public ProtectionType protectionType;
    public override bool CanBeInit()
    {
        return true;
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;

        state = AbilityState.ready;
        isInitialized = true;
    }

    public override bool IsMaxLevel()
    {
        throw new System.NotImplementedException();
    }

    public override void ModifyDebuff(int level) {}

    public override void TriggerAbility() {}

    public override void UpgradeAbility()
    {
        throw new System.NotImplementedException();
    }

    public abstract void AddAction();
    public abstract T Action(int damage);
}
