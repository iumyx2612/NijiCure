using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically Spider Cooking
/// This ability will be treated as Passive since its prefab will be 100% active all the time
/// The Ability state is handled inside the Prefab
/// </summary>
[CreateAssetMenu(menuName = "Ability/Common/Do An Het Han")]
public class DoAnHetHanData : PassiveAbilityBase
{
    public int damage;
    [Range(0f, 1f)] public float critChance;
    public float radiusScale;
    [Range(0f, 1f)] public float slowPercentage;
    public float cooldown;

    public MoveSpeedCounterData counterData;
    
    public GameObject prefab; // This will be added to the player
    private GameObject holder;
    
    public List<DoAnHetHanData> upgradeDatas;

    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentCritChance;
    [HideInInspector] public float currentRadiusScale;
    [HideInInspector] public float currentSlowPercent;
    [HideInInspector] public float currentCooldown;

    public override void Initialize()
    {
        base.Initialize();

        currentDamage = damage;
        currentCritChance = critChance;
        currentRadiusScale = 1;
        currentSlowPercent = slowPercentage;
        currentCooldown = cooldown;
        
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
