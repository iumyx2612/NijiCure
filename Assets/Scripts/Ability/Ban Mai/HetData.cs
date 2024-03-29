using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Ban Mai/Het")]
public class HetData : DamageAbilityBase
{
    [Header("Normal")]
    public float knockbackForce;
    public Vector2 scale;
    public Vector2 offset;
    public GameObject bulletPrefab;

    public List<HetData> upgradeDatas;

    private List<GameObject> pool;

    [HideInInspector] public float currentKnockbackForce;
    [HideInInspector] public Vector2 currentScale;
    
    // Debuff
    [Header("Debuff")] 
    public int bar = 10;

    public override void Initialize()
    {
        base.Initialize();

        currentKnockbackForce = knockbackForce;
        currentScale = scale;
        
        pool = new List<GameObject>();
        GameObject abilityHolder = new GameObject(abilityName + " Holder");
        GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
        bullet.GetComponent<Het>().LoadData(this);
        pool.Add(bullet);
        bullet.SetActive(false);
    }

    public override void TriggerAbility()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            GameObject bullet = pool[i];
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
            }
        }
    }

    public override void UpgradeAbility()
    {
        HetData upgradeData = upgradeDatas[currentLevel];
        // Update current     
        currentCooldownTime = upgradeData.cooldownTime;
        currentKnockbackForce = upgradeData.knockbackForce;
        currentScale = upgradeData.scale;
        currentDamage = upgradeData.damage;

        // Apply upgrade
        for (int i = 0; i < pool.Count; i++)
        {
            GameObject bullet = pool[i];
            bullet.GetComponent<Het>().LoadData(this);
            
            if (currentLevel + 1 >= upgradeDatas.Count)
            {
                bullet.GetComponent<Het>().Awaken();
            }
        }
        currentLevel += 1;
    }

    public override void ModifyDamage(float percentage, bool increase)
    {
        BaseModifyDamage(percentage, increase);
        foreach (GameObject bullet in pool)
        {
            bullet.GetComponent<Het>().LoadData(this);
        }
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
    
    // ----------- For Ngong Ability (Check NgongData.cs) -----------
    public void NgongAbilityUpdate(NgongData _ngongData)
    {
        foreach (GameObject bullet in pool)
        {
            bullet.GetComponent<Het>().LoadNgongData(_ngongData);
        }
    }
}
