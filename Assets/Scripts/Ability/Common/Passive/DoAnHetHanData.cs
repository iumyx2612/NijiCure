using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically Spider Cooking
/// This ability will be treated as Passive since its prefab will be 100% active all the time
/// The Ability state is handled inside the Prefab
/// </summary>
[CreateAssetMenu(menuName = "Ability/Common/Passive/Do An Het Han")]
public class DoAnHetHanData : PassiveAbilityBase
{
    [Header("Specific")]
    public int damage;
    public float radiusScale;
    [Range(0f, 1f)] public float slowPercentage;

    public MoveSpeedCounterData counterData;
    
    public GameObject prefab; // This will be added to the player
    private GameObject holder;
    
    public List<DoAnHetHanData> upgradeDatas;

    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentRadiusScale;
    [HideInInspector] public float currentSlowPercent;

    public override void Initialize()
    {
        base.Initialize();

        currentDamage = damage;
        currentRadiusScale = 1;
        currentSlowPercent = slowPercentage;
        currentCooldownTime = cooldownTime;
        
        // Counter
        counterData.abilityName = abilityName;
        counterData.percentage = currentSlowPercent;
        
        // Holder
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        holder = Instantiate(prefab, player);
        AddAndLoadComponent(holder);
    }

    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.GetComponent<DoAnHetHan>().LoadData(this);
    }

    public override void TriggerAbility() {}

    public override void UpgradeAbility()
    {
        DoAnHetHanData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentDamage = upgradeData.damage;
        currentCooldownTime = upgradeData.cooldownTime;
        currentSlowPercent = upgradeData.slowPercentage;
        currentRadiusScale = upgradeData.radiusScale;
        // Apply upgrade
        counterData.percentage = currentSlowPercent;
        holder.GetComponent<DoAnHetHan>().LoadData(this);
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
