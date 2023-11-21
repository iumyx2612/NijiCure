using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System;


[CreateAssetMenu(menuName = "Ability/Common/StatsBuff/FloatStats")]
public class FloatStatsBuff : AbilityBase
{
    [Header("Buff")]
    public FloatVariable buff;
    [Range(0f, 1f)] public float buffPercent = 0f;
    [Range(0f, 1f)] public float buffFlat = 0;
    [Header("Nerf")]
    public FloatVariable nerf;
    [Range(0f, 1f)] public float nerfPercent;
    [Range(0f, 1f)] public float nerfFlat;
    [Header("Upgrades")]
    public List<FloatStatsBuff> upgradeDatas;

    public override bool CanBeInit()
    {
        return true;
    }

    public override void Initialize()
    {
        buff.Value = (1 + buffPercent) * buff.Value + buffFlat;
        if (nerf != null)
        {
            nerf.Value = nerf.Value / (1 + nerfPercent) - nerfFlat;
        }
        
        state = AbilityState.active;
        isInitialized = true;
    }

    public override void UpgradeAbility()
    {
        FloatStatsBuff upgradeData = upgradeDatas[currentLevel];
        
        buffPercent = upgradeData.buffPercent;
        buffFlat = upgradeData.buffFlat;
        nerfPercent = upgradeData.nerfPercent;
        nerfFlat = upgradeData.nerfFlat;

        buff.Value = (1 + buffPercent) * buff.Value + buffFlat;
        if (nerf != null)
        {
            nerf.Value = nerf.Value / (1 + nerfPercent) - nerfFlat;
        }

        currentLevel += 1;
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
    }

    public override bool IsMaxLevel()
    {
        if (currentLevel >= upgradeDatas.Count && currentLevel >= 1)
        {
            return true;
        }

        return false;
    }

    public override void ModifyDebuff(int level) {}

    public override void TriggerAbility() {}
}
