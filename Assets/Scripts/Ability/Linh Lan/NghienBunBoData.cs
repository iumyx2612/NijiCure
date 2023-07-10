using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Linh Lan/Nghien Bun Bo")]
// When Linh Lan eats, she will gain damage buff for x seconds
public class NghienBunBoData : PassiveAbilityBase
{
    [Range(0f, 1f)] public int damageIncrease;
    public float duration;
    public AbilityCollection currentAbilities;
    public PassiveAbilityGameEvent activeCountdownImage;

    public List<NghienBunBoData> upgradeDatas;

    // For Upgrade
    [HideInInspector] public float currentDuration;
    [HideInInspector] public int currentDamageIncrease;

    private GameObject player;
    
    public override void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AddAndLoadComponent(player);
        currentDamageIncrease = damageIncrease;
        currentDuration = duration;
    }

    public override void UpgradeAbility()
    {
        NghienBunBoData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentDuration = upgradeData.duration;
        currentDamageIncrease = upgradeData.damageIncrease;
        player.GetComponent<NghienBunBo>().LoadData(this);
    }
    
    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
    }

    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.AddComponent<NghienBunBo>();
        objectToAdd.GetComponent<NghienBunBo>().LoadData(this);
    }
    
    public override bool IsMaxLevel()
    {
        if (currentLevel >= upgradeDatas.Count && currentLevel >= 1)
        {
            return true;
        }

        return false;
    }
}
