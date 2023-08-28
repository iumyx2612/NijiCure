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
    public int numAttack;
    public GameObject bulletPrefab;

    public List<HetData> upgradeDatas;

    private List<GameObject> pool;

    [HideInInspector] public float currentKnockbackForce;
    [HideInInspector] public Vector2 currentScale;
    [HideInInspector] public int currentNumAttack;
    
    // Debuff
    [Header("Debuff")] 
    public int bar = 10;

    public override void Initialize()
    {
        base.Initialize();

        currentKnockbackForce = knockbackForce;
        currentScale = scale;
        currentNumAttack = numAttack;
        
        pool = new List<GameObject>();
        GameObject abilityHolder = new GameObject(abilityName + " Holder");
        GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
        bullet.GetComponent<Het>().LoadData(this);
        pool.Add(bullet);
        bullet.SetActive(false);
    }

    public override void TriggerAbility()
    {
        for (int i = 0; i < currentNumAttack; i++)
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
        
        // Check if we create more bullet
        if (currentNumAttack < upgradeData.numAttack)
        {
            currentNumAttack = upgradeData.numAttack;
            // Spawn more bullet
            GameObject abilityHolder = GameObject.Find(abilityName + " Holder"); // A little computation intensive
            GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
            bullet.GetComponent<Het>().LoadData(this);
            // Set the same direction as the first bullet
            bullet.GetComponent<Het>().oldBaseDirection = pool[0].GetComponent<Het>().oldBaseDirection;
            pool.Add(bullet);
            bullet.SetActive(false);
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
