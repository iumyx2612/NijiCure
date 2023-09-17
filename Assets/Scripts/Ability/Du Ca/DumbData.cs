using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Du Ca/Dumb")]
public class DumbData : PassiveAbilityBase
{
    public IntGameEvent increaseExp;
    public IntGameEvent healPlayer;
    public int numTaken; 
    public int healAmount;
    public int explosiveDamage;
    public float radius; // Unchanged
    public LayerMask enemyMask; // Unchanged

    private GameObject player;
    
    public List<DumbData> upgradeDatas;

    [HideInInspector] public int currentNumTaken;
    [HideInInspector] public int currentHealAmount;
    [HideInInspector] public int currentExplosiveDmg;
    
    public override void Initialize()
    {
        base.Initialize();

        currentNumTaken = numTaken;
        currentHealAmount = healAmount;
        currentExplosiveDmg = explosiveDamage;
        
        player = GameObject.FindGameObjectWithTag("Player");
        AddAndLoadComponent(player);
    }

    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.AddComponent<Dumb>();
        objectToAdd.GetComponent<Dumb>().LoadData(this);
    }

    public override void TriggerAbility()
    {
        base.TriggerAbility();
    }
    
    public override void UpgradeAbility()
    {
        DumbData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentNumTaken = upgradeData.numTaken;
        currentExplosiveDmg = upgradeData.explosiveDamage;
        currentHealAmount = upgradeData.healAmount;
        // Load
        player.GetComponent<Dumb>().LoadData(this);
        // Increase level
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
