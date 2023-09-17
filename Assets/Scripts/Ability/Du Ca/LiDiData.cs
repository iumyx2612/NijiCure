using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Du Ca/Li Di")]
public class LiDiData : PassiveAbilityBase
{
    public ChongQuocDanData dependentAbility;
    
    public int healAmount;
    public int explosiveDamage;

    public GameObjectCollection cukaPool;

    [HideInInspector] public int currentHealAmount;
    [HideInInspector] public int currentExplosiveDamage;

    public override void Initialize()
    {
        base.Initialize();

        currentHealAmount = healAmount;
        currentExplosiveDamage = explosiveDamage;
        
        // Parse the data to all the Married Cuka
        for (int i = 0; i < cukaPool.Count; i++)
        {
            cukaPool[i].GetComponent<MarriedCuka>().LoadLiDiData(this);
        }
    }

    public override bool CanBeInit()
    {
        if (!dependentAbility.isInitialized)
            return false;
        return true;
    }

    public override void AddAndLoadComponent(GameObject objectToAdd) {}

    public override void TriggerAbility() {}

    public override void UpgradeAbility()
    {
        base.UpgradeAbility();
    }

    public override bool IsMaxLevel()
    {
        throw new System.NotImplementedException();
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        throw new System.NotImplementedException();
    }
}
