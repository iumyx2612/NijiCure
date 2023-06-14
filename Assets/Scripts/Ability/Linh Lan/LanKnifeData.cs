using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Linh Lan/Lan Knife")]
public class LanKnifeData : DamageAbilityBase
{
    public Sprite sprite;
    public float knifeSpeed;
    public float knifeDistance;
    public float knifeScale;
    public GameObject bulletPrefab;

    public List<LanKnifeData> upgradeDatas;

    [HideInInspector] public List<GameObject> pool;
    
    // For Upgrade
    [HideInInspector] public float currentKnifeScale;  
    [HideInInspector] public float currentKnifeDistance;  
    [HideInInspector] public float currentKnifeSpeed;  

    // Fixed
    [HideInInspector] public Vector2 colliderSize = new Vector2(0.2f, 0.15f);

    public override void Initialize()
    {
        currentLevel = 0;
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;

        currentKnifeScale = knifeScale;
        currentKnifeDistance = knifeDistance;
        currentKnifeSpeed = knifeSpeed;
        currentDamage = damage;

        state = AbilityState.cooldown;
        
        pool = new List<GameObject>();
        GameObject abilityHolder = new GameObject(abilityName + " Holder");
        GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
        bullet.GetComponent<LanKnife>().LoadData(this);
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
        LanKnifeData upgradeData = upgradeDatas[currentLevel];
        // Update current     
        currentCooldownTime = upgradeData.cooldownTime;
        currentKnifeDistance = upgradeData.knifeDistance;
        currentKnifeScale = upgradeData.knifeScale;
        currentKnifeSpeed = upgradeData.knifeSpeed;
        currentDamage = upgradeData.damage;
        
        GameObject bullet = pool[0];
        bullet.GetComponent<LanKnife>().LoadData(this);
        currentLevel += 1;
    }
    
    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
    }

    public override void PartialModify(int value)
    {
        GameObject bullet = pool[0];
        bullet.GetComponent<LanKnife>().ModifyDamage(value);
    }
}
