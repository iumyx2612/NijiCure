using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Linh Lan/Chua Te Nua Bai")]
public class ChuaTeNuaBaiData : DamageAbilityBase
{
    public Sprite sprite;
    public float scale;
    public GameObject prefab;
    
    public List<ChuaTeNuaBaiData> upgradeDatas;

    [HideInInspector] public List<GameObject> pool;
    
    // For Upgrade
    [HideInInspector] public float currentScale;
    
    
    public override void Initialize()
    {
        currentLevel = 0;
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;

        currentDamage = damage;
        currentScale = scale;
        
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
        Debug.Log(currentLevel);
        return upgradeDatas[currentLevel];
    }

    public override void PartialModify(int value)
    {
        GameObject bullet = pool[0];
        bullet.GetComponent<ChuaTeNuaBai>().ModifyDamage(value);
    }
}
