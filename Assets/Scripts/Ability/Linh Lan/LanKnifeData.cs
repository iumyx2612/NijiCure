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
    [HideInInspector] public float currentKnifeScale; // For upgrade 
    [HideInInspector] public float currentKnifeDistance; // For upgrade 
    [HideInInspector] public float currentKnifeSpeed; // For upgrade 

    // Fixed
    [HideInInspector] public Vector2 colliderSize = new Vector2(0.2f, 0.15f);

    public override void Initialize()
    {
        currentLevel = 0;
        currentCooldownTime = cooldownTime;

        currentKnifeScale = knifeScale;
        currentKnifeDistance = knifeDistance;
        currentKnifeSpeed = knifeSpeed;
        currentDamage = damage;
        
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
        bullet.GetComponent<LanKnife>().LoadData(upgradeData);
        currentLevel += 1;
    }

    public override void PartialModify(int value)
    {
        GameObject bullet = pool[0];
        bullet.GetComponent<LanKnife>().ModifyDamage(value);
    }
}
