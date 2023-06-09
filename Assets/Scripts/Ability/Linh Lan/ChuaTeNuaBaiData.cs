using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Linh Lan/Chua Te Nua Bai")]
public class ChuaTeNuaBaiData : DamageAbilityBase
{
    public Sprite sprite;
    public float scale;
    public GameObject prefab;
    
    public List<ChuaTeNuaBaiData> upgradedDatas;

    [HideInInspector] public List<GameObject> pool;
    
    
    public override void Initialize()
    {
        currentLevel = 0;
        currentCooldownTime = cooldownTime;
        state = AbilityState.ready;
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
        ChuaTeNuaBaiData upgradeData = upgradedDatas[currentLevel];
        currentCooldownTime = upgradeData.cooldownTime;
        GameObject bullet = pool[0];
        bullet.GetComponent<ChuaTeNuaBai>().LoadData(upgradeData);
        currentLevel += 1;
    }

    public override void PartialModify(int value)
    {
        GameObject bullet = pool[0];
        bullet.GetComponent<ChuaTeNuaBai>().ModifyDamage(value);
    }
}
