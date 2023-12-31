using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ban Mai/Rage")]
public class RageData : PassiveAbilityBase
{
    public HetData baseHetData;
    public float buffTime;
    [Range(0f, 1f)] public float damageBuff;
    public IntGameEvent playerTakeDamage; 
    public PassiveAbilityGameEvent activeCountdownImage; 

    public List<RageData> upgradeDatas;

    [HideInInspector] public float currentDamageBuff;

    // State
    private bool havingBuff;

    public override void PreInit()
    {
        base.PreInit();
        playerTakeDamage.RemoveListener(Buff);
    }

    public override void Initialize()
    {
        base.Initialize();
        playerTakeDamage.AddListener(Buff);
        currentCooldownTime = buffTime;
        currentDamageBuff = damageBuff;
    }    

    private void Buff()
    {
        activeCountdownImage.Raise(new PassiveAbilityInfo(currentCooldownTime, abilityIcon, 0, false, false));
        internalCooldownTime = 0f;
        state = AbilityState.cooldown;
        if (!havingBuff)
            baseHetData.ModifyDamage(currentDamageBuff, true);
        havingBuff = true;
    }
    
    public override void AddAndLoadComponent(GameObject objectToAdd) {}

    public override void TriggerAbility() {}

    public override void UpgradeAbility()
    {
        RageData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentDamageBuff = upgradeData.damageBuff;
        currentCooldownTime = upgradeData.buffTime;

        currentLevel += 1;
    }

    public override bool IsMaxLevel()
    {
        if (currentLevel >= upgradeDatas.Count && currentLevel >= 1)
        {
            return true;
        }

        return false;
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
    }
}
