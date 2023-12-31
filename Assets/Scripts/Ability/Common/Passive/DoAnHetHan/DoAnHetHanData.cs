using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically Spider Cooking
/// This ability will be treated as Passive since its prefab will be 100% active all the time
/// The Ability state is handled inside the Prefab
/// Tăng sát thương người chơi nhận vào thêm 10%
/// </summary>
[CreateAssetMenu(menuName = "Ability/Common/Passive/Do An Het Han")]
public class DoAnHetHanData : PassiveAbilityBase
{
    [Header("Do An Het Han")]
    public int damage;
    public float radiusScale;

    [Header("Slow Enemy")]
    [SerializeField] private MoveSpeedCounter counter;
    [Header("Increase dmg on self")]
    [SerializeField] private DamageBuffCounter selfCounter;
    
    public GameObject prefab; // This will be added to the player
    private GameObject holder;
    
    public List<DoAnHetHanData> upgradeDatas;

    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentRadiusScale;
    [HideInInspector] public MoveSpeedCounter currentCounter;
    [HideInInspector] public DamageBuffCounter currentSelfCounter;

    public override void Initialize()
    {
        base.Initialize();

        currentDamage = damage;
        currentRadiusScale = 1;
        currentCooldownTime = cooldownTime;
        currentCounter = new MoveSpeedCounter(counter);
        currentSelfCounter = new DamageBuffCounter(selfCounter);
        
        
        // Holder
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        holder = Instantiate(prefab, player);
        AddAndLoadComponent(holder);
        player.GetComponent<PlayerCounter>().AddDmgBuffCounter(currentSelfCounter);
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
        currentRadiusScale = upgradeData.radiusScale;
        currentCounter = new MoveSpeedCounter(upgradeData.counter);
        // Apply upgrade
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
