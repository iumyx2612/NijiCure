using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Linh Lan/Chua Te Nua Bai")]
public class ChuaTeNuaBaiData : DamageAbilityBase
{
    public float scale;
    public GameObject prefab;
    
    public List<ChuaTeNuaBaiData> upgradeDatas;

    private List<GameObject> pool;
    
    // For Upgrade
    [HideInInspector] public float currentScale;
    
    
    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;

        // Runtime data
        currentDamage = damage;
        currentScale = scale;
        currentCritChance = critChance;
        
        state = AbilityState.cooldown;
        
        pool = new List<GameObject>();
        GameObject abilityHolder = new GameObject( " Holder");
        GameObject bullet = Instantiate(prefab, abilityHolder.transform);
        bullet.GetComponent<ChuaTeNuaBai>().LoadData(this);
        pool.Add(bullet);
        bullet.SetActive(false);

    }

    public override void TriggerAbility()
    {
        GameObject bullet = pool[0];
        bullet.SetActive(true);
    }

    public override void UpgradeAbility()
    {
        ChuaTeNuaBaiData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentCooldownTime = upgradeData.cooldownTime;
        currentDamage = upgradeData.damage;
        currentScale = upgradeData.scale;
        
        GameObject bullet = pool[0];
        bullet.GetComponent<ChuaTeNuaBai>().LoadData(this);
        currentLevel += 1;
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
    }

    public override void ModifyDamage(float percentage, bool increase)
    {
        BaseModifyDamage(percentage, increase);
        GameObject bullet = pool[0];
        bullet.GetComponent<ChuaTeNuaBai>().LoadData(this);
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
