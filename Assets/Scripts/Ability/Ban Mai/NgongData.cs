using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Kẻ địch khi nhận sát thương từ Het có khả năng bị dính Ngọng counter
/// Sát thương của Hét lên kẻ địch có Ngọng được tăng lên
/// </summary>
[CreateAssetMenu(menuName = "Ability/Ban Mai/Ngong")]
public class NgongData : PassiveAbilityBase
{
    [Header("Normal")]
    [Range(0, 1)] public float placeChance;

    [Header("Counter")] 
    public DamageBuffCounter counter;
    
    public HetData baseHetData;
    
    public List<NgongData> upgradeDatas;

    [HideInInspector] public float currentPlaceChance;
    [HideInInspector] public DamageBuffCounter currentCounter;

    public override void Initialize()
    {
        base.Initialize();
        currentPlaceChance = placeChance;
        currentCounter = counter; // Initialize as a reference
        
        // Init the Counter GameObject that holds animation
        currentCounter.InitCounterObject();
        
        // Parse the data to Ngong
        baseHetData.NgongAbilityUpdate(this);
    }

    public override void AddAndLoadComponent(GameObject objectToAdd) {}

    public override void UpgradeAbility()
    {
        NgongData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentPlaceChance = upgradeData.placeChance;
        currentCounter = new DamageBuffCounter(upgradeData.counter); // Upgrade datas 
        // Apply upgrade on Bullet
        baseHetData.NgongAbilityUpdate(this);

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
