using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Kẻ địch khi nhận sát thương từ Het có khả năng bị dính Ngọng counter
/// Sát thương của Hét lên kẻ địch có Ngọng được tăng lên theo số Counter
/// </summary>
[CreateAssetMenu(menuName = "Ability/Ban Mai/Ngong")]
public class NgongData : PassiveAbilityBase
{
    [Header("Normal")]
    [Range(0, 1)] public float damageBuff;
    [Range(0, 1)] public float placeChance;

    [Header("Counter")] 
    public DamageBuffCounter dmgBuffCounter;
    public float counterDuration;
    public int maxNumCounter;
    public GameObject counterPrefab;
    private List<GameObject> counterPool;
    
    public HetData baseHetData;
    
    public List<NgongData> upgradeDatas;

    [HideInInspector] public float currentDamageBuff;
    [HideInInspector] public float currentPlaceChance;
    [HideInInspector] public float currentCounterDuration;
    [HideInInspector] public int currentMaxNumCounter;

    public override void Initialize()
    {
        base.Initialize();
        currentDamageBuff = damageBuff;
        currentPlaceChance = placeChance;
        currentCounterDuration = counterDuration;
        currentMaxNumCounter = maxNumCounter;
        
        // Init the Counter GameObject that holds animation\
        counterPool = new List<GameObject>();
        GameObject counterHolder = new GameObject(abilityName + " Counters");
        for (int i = 0; i < 20; i++)
        {
            GameObject counter = Instantiate(counterPrefab, counterHolder.transform);
            counterPool.Add(counter);
            counter.SetActive(false);
        }
        
        // Parse the data to Ngong
        baseHetData.NgongAbilityUpdate(this);
        
        // Parse the data to Counter
        dmgBuffCounter.abilityName = abilityName;
        dmgBuffCounter.existTime = currentCounterDuration;
        dmgBuffCounter.maxNum = currentMaxNumCounter;
        dmgBuffCounter.counterPrefab = counterPrefab;
        dmgBuffCounter.counterPool.Clear();
        dmgBuffCounter.counterPool = counterPool;
    }

    public override void AddAndLoadComponent(GameObject objectToAdd) {}

    public override void UpgradeAbility()
    {
        NgongData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentCounterDuration = upgradeData.counterDuration;
        currentDamageBuff = upgradeData.damageBuff;
        currentPlaceChance = upgradeData.placeChance;
        currentMaxNumCounter = upgradeData.maxNumCounter;
        // Apply upgrade on Bullet
        baseHetData.NgongAbilityUpdate(this);
        // Apply upgrade on Counter
        dmgBuffCounter.existTime = currentCounterDuration;
        dmgBuffCounter.maxNum = currentMaxNumCounter;
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
