using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Ability/Linh Lan/Lan Knife")]
public class LanKnifeData : DamageAbilityBase
{
    [Header("Normal")]
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
    
    // Debuff
    [Header("Debuff")]
    public string debuffDescription;
    public int numToDebuff;
    private int internalNumToDebuff;

    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;

        currentKnifeScale = knifeScale;
        currentKnifeDistance = knifeDistance;
        currentKnifeSpeed = knifeSpeed;
        currentDamage = damage;
        currentCritChance = critChance;

        internalNumToDebuff = 0;

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
        internalNumToDebuff += 1;
        GameObject bullet = pool[0];
        bullet.SetActive(true);   
        // Debuff Attack
        if (internalNumToDebuff >= numToDebuff)
        {
            Vector2 randomDirection = new Vector2();
            // Random direction
            if (Random.value < 0.5f)
            {
                randomDirection = new Vector2(1, Random.Range(-1, 1));
            }
            else
            {
                randomDirection = new Vector2(-1, Random.Range(-1, 1));
            }
            bullet.GetComponent<LanKnife>().bulletDirection = randomDirection;
            internalNumToDebuff = 0;
        }
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

    public override void ModifyDamage(float percentage, bool increase)
    {
        BaseModifyDamage(percentage, increase);
        GameObject bullet = pool[0];
        bullet.GetComponent<LanKnife>().LoadData(this);
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
