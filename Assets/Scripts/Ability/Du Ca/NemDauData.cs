using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


/// <summary>
/// This is basically Okayu's starting ability
/// Du Ca can hurt herself cuz she's Ngố
/// </summary>
public class NemDauData : DamageAbilityBase
{
    public List<float> angles;
    public Vector2 scale;
    public int numBullets;
    public GameObject bulletPrefab;

    public List<NemDauData> upgradeDatas;

    [HideInInspector] public List<GameObject> pool;

    [HideInInspector] public List<float> currentAngles;
    [HideInInspector] public Vector2 currentScale;
    [HideInInspector] public int currentNumBullets;

    // Debuff
    [Range(0f, 1f)] public float selfDamageChance;
    public IntGameEvent playerTakeDamage; // Setup in PlayerCombat.cs
    
    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;
        state = AbilityState.cooldown;

        currentDamage = damage;
        currentCritChance = critChance;
        
        List<float> temp = new List<float>(angles);
        currentAngles = temp;
        currentScale = scale;
        currentNumBullets = numBullets;
        
        pool = new List<GameObject>();
        GameObject abilityHolder = new GameObject(abilityName + " Holder");
        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, abilityHolder.transform);
            bullet.GetComponent<NemDau>().LoadData(this, i);
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
            playerTakeDamage.Raise((int)(0.1 * currentDamage));
        }
    }

    public override void UpgradeAbility()
    {
        base.UpgradeAbility();
    }

    public override void ModifyDamage(float percentage, bool increase)
    {
        throw new System.NotImplementedException();
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsMaxLevel()
    {
        throw new System.NotImplementedException();
    }
}
