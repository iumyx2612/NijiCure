using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Kẻ địch trong phạm vi `radius` khi bị hạ gục sẽ có khả
/// năng drop ra Pudding `itemDropCounter`
/// Nhặt Pudding sẽ hồi máu + buff sát thương của Hét -> Implement trong Items/BanMaiPuddingItemDrop.cs
/// </summary>
[CreateAssetMenu(menuName = "Ability/Ban Mai/Pudding Hunter")]
public class PuddingHunterData : PassiveAbilityBase
{
    public float radius;
    [Range(0, 1)] public float damageIncrease;
    public ItemDropCounter itemDropCounter;

    public List<PuddingHunterData> upgradeDatas;

    public override void Initialize()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject holder = Instantiate(new GameObject(abilityName + " Holder"),
            player);
        holder.transform.localPosition = Vector2.zero;
        AddAndLoadComponent(holder);
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
    
    public override void UpgradeAbility() {}

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
