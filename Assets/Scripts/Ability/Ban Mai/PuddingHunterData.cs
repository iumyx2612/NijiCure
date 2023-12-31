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
    [Header("Pudding Hunter")]
    public float radius; // To PuddingHunter.cs
    public float buffTime; // To PuddingHunter.cs
    public GameEvent puddingHunterGameEvent; // To PuddingHunter.cs
    public HetData baseHetAbility; // To PuddingHunter.cs
    [Range(0f, 1f)] public float damageIncrease; // To PuddingHunter.cs
    public int healthIncrease; // To BanMaiPudding.cs

    [Header("UI")]
    public PassiveAbilityGameEvent activeCountdownImage; // To PuddingHunter.cs
    
    // Pudding Counter
    public GameObject puddingPrefab;
    private GameObject holder;
    public ItemDropCounter counter;

    public List<PuddingHunterData> upgradeDatas;

    [HideInInspector] public float currentRadius;
    [HideInInspector] public float currentBuffTime;
    [HideInInspector] public float currentDamageIncrease;
    [HideInInspector] public int currentHealthIncrease;
    [HideInInspector] public ItemDropCounter currentCounter;

    public override void Initialize()
    {
        base.Initialize();

        currentRadius = radius;
        currentBuffTime = buffTime;
        currentDamageIncrease = damageIncrease;
        currentHealthIncrease = healthIncrease;
        currentCounter = counter;
        currentCounter.InitCounterObject();
        for (int i = 0; i < currentCounter.counterPool.Count; i++)
        {
            currentCounter.counterPool[i].GetComponent<BanMaiPudding>().LoadData(this);
        }
        
        // Attach the GameObject with PuddingHunter script to Player
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject _holder = new GameObject(abilityName + " Holder");
        _holder.transform.parent = player;
        _holder.transform.localPosition = Vector2.zero;
        AddAndLoadComponent(_holder);
        holder = _holder;
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
        currentHealthIncrease = upgradeData.healthIncrease;
        
        ItemDropCounter tempCounter = currentCounter;
        currentCounter = new ItemDropCounter(upgradeData.counter);
        currentCounter.LinkPool(tempCounter);

        // Apply the upgrade on PuddingHunter.cs
        holder.GetComponent<PuddingHunter>().LoadData(this);
        // Apply the upgrade on BanMaiPudding.cs
        for (int i = 0; i < currentCounter.counterPool.Count; i++)
        {
            currentCounter.counterPool[i].GetComponent<BanMaiPudding>().LoadData(this);
        }
        // Apply the upgrade on ItemDropCounterData.cs

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
