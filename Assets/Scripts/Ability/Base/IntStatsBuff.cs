using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[CreateAssetMenu(menuName = "Ability/Common/StatsBuff/IntStats")]
public class IntStatsBuff : AbilityBase
{
    [Header("Buff")]
    public IntVariable buff;
    [Range(0f, 1f)] public float buffPercent = 0f;
    public int buffFlat = 0;
    [Header("Nerf")]
    public IntVariable nerf;
    [Range(0f, 1f)] public float nerfPercent;
    public int nerfFlat;
    [Header("Upgrades")]
    public List<IntStatsBuff> upgradeDatas;
    public override bool CanBeInit()
    {
        return true;
    }

    public override void Initialize()
    {
        buff.Value = Mathf.RoundToInt((1 + buffPercent) * buff.Value) + buffFlat;
        if (nerf != null)
        {
            nerf.Value = Mathf.RoundToInt(nerf.Value / (1 + nerfPercent)) - nerfFlat;
        }
        
        state = AbilityState.active;
        isInitialized = true;
    }

    public override void UpgradeAbility()
    {
        IntStatsBuff upgradeData = upgradeDatas[currentLevel];
        
        buffPercent = upgradeData.buffPercent;
        buffFlat = upgradeData.buffFlat;
        nerfPercent = upgradeData.nerfPercent;
        nerfFlat = upgradeData.nerfFlat;

        buff.Value = Mathf.RoundToInt((1 + buffPercent) * buff.Value) + buffFlat;
        if (nerf != null)
        {
            nerf.Value = Mathf.RoundToInt(nerf.Value / (1 + nerfPercent)) - nerfFlat;
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
