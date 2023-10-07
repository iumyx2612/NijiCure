using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using MathHelper;


[CreateAssetMenu(menuName = "Ability/Common/Damage/NijiBomb")]
public class NijiBombData : DamageAbilityBase
{
    public Vector2 spawnArea; // Can't be upgraded
    public Vector2Variable playerPosRef;
    public float explosiveRadius;
    public int numBombs; 
    public GameObject bombPrefab;
    private GameObject bombHolder;
    private List<GameObject> bombPool;

    public List<NijiBombData> upgradeDatas;

    [HideInInspector] public float currentExplosiveRadius;
    [HideInInspector] public int currentNumBombs;


    public override void Initialize()
    {
        base.Initialize();

        currentExplosiveRadius = explosiveRadius;
        currentNumBombs = numBombs;

        bombHolder = new GameObject("NijiBomb Holder");
        bombPool = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject nijiBomb = Instantiate(bombPrefab, bombHolder.transform);
            nijiBomb.GetComponent<NijiBomb>().LoadData(this);
            bombPool.Add(nijiBomb);
            nijiBomb.SetActive(false);
        }
    }

    public override void TriggerAbility()
    {
        for (int i = 0; i < currentNumBombs; i++)
        {
            GameObject nijiBomb = bombPool[i];
            if (!nijiBomb.activeSelf)
            {
                Vector2 position = PositionSampling.RandomPositionInSquare(playerPosRef.Value, spawnArea);
                Debug.Log(position);
                nijiBomb.SetActive(true);
                nijiBomb.transform.position = position;
            }
        }
    }

    public override void ModifyDamage(float percentage, bool increase)
    {
        BaseModifyDamage(percentage, increase);
        for (int i = 0; i < bombPool.Count; i++)
        {
            bombPool[i].GetComponent<NijiBomb>().LoadData(this);
        }
    }

    public override void UpgradeAbility()
    {
        NijiBombData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentCooldownTime = upgradeData.cooldownTime;
        currentDamage = upgradeData.damage;
        currentExplosiveRadius = upgradeData.explosiveRadius;
        currentNumBombs = upgradeData.numBombs;
        // Apply upgrade
        for (int i = 0; i < bombPool.Count; i++)
        {
            bombPool[i].GetComponent<NijiBomb>().LoadData(this);
        }
        // Increase level
        currentLevel += 1;
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
}
