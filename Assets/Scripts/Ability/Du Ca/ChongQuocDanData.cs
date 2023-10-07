using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using MathHelper;


/// <summary>
/// Every x seconds, create a zone that has x% to convert targets inside the zone
/// to Married Cuka
/// Married Cuka can't move, deals x damage every 0.x seconds for target within x range
/// Married Cuka disappears after x seconds
/// </summary>
[CreateAssetMenu(menuName = "Ability/Du Ca/Chong Quoc Dan")]
public class ChongQuocDanData : DamageAbilityBase
{
    [Header("Zone")]
    public GameObject zonePrefab;
    public Vector2 zoneDistFromPlayer; // Unchanged
    [Range(0f, 1f)] public float convertChance;
    public float zoneRadius;

    [Header("Cuka")] 
    public GameObject cukaPrefab;
    public int cukaDamage;
    public float cukaRange; // Unchanged
    public float cukaAttackTimer;
    public float cukaLifeTime; // Unchanged

    public Vector2Variable playerPosRef; // Track player's pos to activate zone
    
    public GameObjectCollection cukaPool;
    private GameObject cukaHolder;
    private GameObject zoneGameObject;

    public List<ChongQuocDanData> upgradeDatas;

    [HideInInspector] public float currentConvertChance;
    [HideInInspector] public float currentZoneRadius;
    [HideInInspector] public int currentCukaDamage;
    [HideInInspector] public float currentCukaAtkTimer;
    
    public override void Initialize()
    {
        base.Initialize();

        currentConvertChance = convertChance;
        currentZoneRadius = 1;
        currentCukaDamage = cukaDamage;
        currentCukaAtkTimer = cukaAttackTimer;
        
        // Married Cuka pool
        cukaPool.Clear();
        cukaHolder = new GameObject("MarriedCukaHolder");
        for (int i = 0; i < 20; i++)
        {
            GameObject cuka = Instantiate(cukaPrefab, cukaHolder.transform);
            cuka.GetComponent<MarriedCuka>().LoadData(this);
            cukaPool.Add(cuka);
            cuka.SetActive(false);
        }
        
        // Init the zone
        zoneGameObject = Instantiate(zonePrefab);
        zoneGameObject.GetComponent<ChongQuocDan>().LoadData(this);
        zoneGameObject.SetActive(false);
    }

    public override void TriggerAbility()
    {
        // Select a random position within the distance from player
        Vector2 pos = PositionSampling.RandomPositionInSquare(playerPosRef.Value, zoneDistFromPlayer);
        // Active the Zone, state is changed to cooldown in ChongQuocDan.cs
        zoneGameObject.transform.position = pos;
        zoneGameObject.SetActive(true);
    }

    public override void UpgradeAbility()
    {
        ChongQuocDanData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentConvertChance = upgradeData.convertChance;
        currentZoneRadius = upgradeData.zoneRadius;
        currentCukaDamage = upgradeData.cukaDamage;
        currentCukaAtkTimer = upgradeData.cukaAttackTimer;
        currentDamage = upgradeData.currentDamage;
        // Perform upgrade on ChongQuocDan.cs
        zoneGameObject.GetComponent<ChongQuocDan>().LoadData(this);
        // Perform upgrade on MarriedCuka
        for (int i = 0; i < cukaPool.Count; i++)
        {
            cukaPool[i].GetComponent<MarriedCuka>().LoadData(this);
        }
        // Increase level
        currentLevel += 1;
    }

    // This ability damage can not be increased
    public override void ModifyDamage(float percentage, bool increase)
    {
        
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

