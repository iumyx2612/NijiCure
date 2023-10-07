using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


/// <summary>
/// This is basically Okayu's starting ability
/// Du Ca can hurt herself cuz she's Ngố
/// </summary>
[CreateAssetMenu(menuName = "Ability/Du Ca/Nem Dau")]
public class NemDauData : DamageAbilityBase
{
    public Vector2 scale;
    public int numBullets;
    public GameObject bulletPrefab;

    public List<NemDauData> upgradeDatas;

    [HideInInspector] public List<GameObject> pool;

    [HideInInspector] public Vector2 currentScale;
    [HideInInspector] public int currentNumBullets;

    // Debuff
    [Range(0f, 1f)] public float selfDamageChance;
    public IntGameEvent playerTakeDamage; // Setup in PlayerCombat.cs
    
    public override void Initialize()
    {
        base.Initialize();
        
        currentScale = scale;
        currentNumBullets = numBullets;
        
        pool = new List<GameObject>();
        GameObject abilityHolder = new GameObject(abilityName + " Holder");
        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
            bullet.GetComponent<NemDau>().LoadData(this);
            pool.Add(bullet);
            bullet.SetActive(false);
        }
    }

    public override void TriggerAbility()
    {
        for (int i = 0; i < currentNumBullets; i++)
        {
            GameObject bullet = pool[i];
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
            }
        }

        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < selfDamageChance)
        {
            playerTakeDamage.Raise((int)(0.2 * currentDamage));
        }
    }

    public override void UpgradeAbility()
    {
        NemDauData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentDamage = upgradeData.damage;
        currentCooldownTime = upgradeData.cooldownTime;
        currentScale = upgradeData.scale;
        if (upgradeData.numBullets > currentNumBullets)
        {
            GameObject abilityHolder = GameObject.Find(abilityName + " Holder");
            GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
            pool.Add(bullet);
            bullet.SetActive(false);
        }
        currentNumBullets = upgradeData.numBullets;
        // Apply
        foreach (GameObject bullet in pool)
        {
            bullet.GetComponent<NemDau>().LoadData(this);
        }

        currentLevel += 1;
    }

    public override void ModifyDamage(float percentage, bool increase)
    {
        BaseModifyDamage(percentage, increase);
        foreach (GameObject bullet in pool)
        {
            bullet.GetComponent<NemDau>().LoadData(this);
        }
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
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
