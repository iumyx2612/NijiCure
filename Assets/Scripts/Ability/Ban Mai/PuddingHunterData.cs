using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


/// <summary>
/// Kẻ địch trong phạm vi `radius` khi bị hạ gục sẽ có khả
/// năng drop ra `itemDropCounter` Pudding
/// Nhặt Pudding sẽ hồi máu + buff sát thương của Hét -> Implement trong Items/BanMaiPudding.cs
///
/// How this Ability works:
/// - Setup the Pudding Drop pool + Counter data using PuddingHunterData
/// - Setup the empty GameObject with PuddingHunter.cs as child of Player -> so it moves with Player;
/// - The empty GameObject acts as a `radius` detector, check PuddingHunter.cs
/// - Counter will drop Pudding with Items/BanMaiPudding.cs
/// </summary>
[CreateAssetMenu(menuName = "Ability/Ban Mai/Pudding Hunter")]
public class PuddingHunterData : PassiveAbilityBase
{
    [Header("Normal")]
    public float radius; // To PuddingHunter.cs
    public float buffTime; // To PuddingHunter.cs
    public GameEvent puddingHunterGameEvent; // To PuddingHunter.cs
    public HetData baseHetAbility; // To PuddingHunter.cs
    [Range(0f, 1f)] public float damageIncrease; // To PuddingHunter.cs
    public int healthIncrease; // To BanMaiPudding.cs
    [Range(0f, 1f)] public float dropChance; // To ItemDropCounterData.cs

    [Header("UI")]
    public PassiveAbilityGameEvent activeCountdownImage; // To PuddingHunter.cs
    
    public GameObject puddingPrefab;
    private GameObject holder;
    private List<GameObject> puddingPool;
    public ItemDropCounterData itemDropCounterData;

    public List<PuddingHunterData> upgradeDatas;

    [HideInInspector] public float currentRadius;
    [HideInInspector] public float currentBuffTime;
    [HideInInspector] public float currentDamageIncrease;
    [HideInInspector] public int currentHealthIncrease;
    [HideInInspector] public float currentDropChance;

    public override void Initialize()
    {
        base.Initialize();

        currentRadius = radius;
        currentBuffTime = buffTime;
        currentDamageIncrease = damageIncrease;
        currentHealthIncrease = healthIncrease;
        currentDropChance = dropChance;
        
        // Parse the data to Counter
        itemDropCounterData.abilityName = abilityName;
        itemDropCounterData.itemDropPrefab = puddingPrefab;
        itemDropCounterData.itemDropPool.Clear();
        itemDropCounterData.itemDropPool = puddingPool;
        itemDropCounterData.dropChance = currentDropChance;
        
        // Attach the GameObject with PuddingHunter script to Player
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject _holder = new GameObject(abilityName + " Holder");
        _holder.transform.parent = player;
        _holder.transform.localPosition = Vector2.zero;
        AddAndLoadComponent(_holder);
        holder = _holder;
        
        // Pool the item drop
        puddingPool = new List<GameObject>();
        GameObject counterHolder = new GameObject(abilityName + " Counters");
        for (int i = 0; i < 20; i++)
        {
            GameObject itemDrop = Instantiate(puddingPrefab, counterHolder.transform);
            itemDrop.GetComponent<BanMaiPudding>().LoadData(this);
            puddingPool.Add(itemDrop);
            itemDrop.SetActive(false);
        }
    }

    /// <summary>
    /// Create a child GameObject of Player which acts as a Radius zone
    /// Attach PuddingHunter.cs to that GameObject
    /// </summary>
    /// <param name="objectToAdd"></param>
    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.AddComponent<PuddingHunter>();
        objectToAdd.GetComponent<PuddingHunter>().LoadData(this);
    }
    
    public override void TriggerAbility() {}

    public override void UpgradeAbility()
    {
        PuddingHunterData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentRadius = upgradeData.radius;
        currentBuffTime = upgradeData.buffTime;
        currentDamageIncrease = upgradeData.damageIncrease;
        currentDropChance = upgradeData.dropChance;
        currentHealthIncrease = upgradeData.healthIncrease;
        // Apply the upgrade on PuddingHunter.cs
        holder.GetComponent<PuddingHunter>().LoadData(this);
        // Apply the upgrade on BanMaiPudding.cs
        foreach (GameObject pudding in puddingPool)
        {
            pudding.GetComponent<BanMaiPudding>().LoadData(this);
        }
        // Apply the upgrade on ItemDropCounterData.cs
        itemDropCounterData.dropChance = currentDropChance;

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
