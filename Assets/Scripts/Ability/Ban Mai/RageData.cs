using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ban Mai/Rage")]
public class RageData : PassiveAbilityBase
{
    public float buffTime;
    [Range(0f, 1f)] public float damageBuff;
    public Vector2 scaleBuff;
    public IntGameEvent playerTakeDamage; // To Assign to Rage.cs
    public HetData baseHetData; // To Assign to Rage.cs
    public PassiveAbilityGameEvent activeCountdownImage; // To Assign to Rage.cs

    public List<RageData> upgradeDatas;

    [HideInInspector] public float currentBuffTime;
    [HideInInspector] public float currentDamageBuff;
    [HideInInspector] public Vector2 currentScaleBuff;
    

    public override void Initialize()
    {
        base.Initialize();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AddAndLoadComponent(player);
        currentBuffTime = buffTime;
        currentDamageBuff = damageBuff;
        currentScaleBuff = scaleBuff;
    }    
    
    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.AddComponent<Rage>();
        objectToAdd.GetComponent<Rage>().LoadData(this);
    }

    public override void TriggerAbility()
    {
        
    }

    public override void UpgradeAbility()
    {
        base.UpgradeAbility();
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
