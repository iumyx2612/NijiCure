using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// Mỗi x (s) tăng 1 stack Dẹo (max 5), mỗi stack dẹo tăng dmg base ability thêm x%
/// Khi chưa max stack Dẹo thì bị slow x%
/// Khi trúng đòn thì mất toàn bộ stack, phải đợi x (s) thì mới tăng stack
/// </summary>
/// 
[CreateAssetMenu(menuName = "Ability/Common/Passive/Deo")]
public class DeoData : PassiveAbilityBase
{
    [Header("Deo")]
    public int stackPerSec;
    public float cooldownWhenHit; 
    public int maxStacks;
    [Range(0f, 1f)] public float buffPercent;
    public PlayerData stagePlayerData;
    [HideInInspector] public DamageAbilityBase startingAbility;
    private GameObject player;

    [Header("Reference to Player")]
    public MoveSpeedCounter counter;
    public IntGameEvent playerTakeDamage;
    [Header("Visual")]
    public PassiveAbilityGameEvent activeCountdownImage;

    public List<DeoData> upgradeDatas;

    [HideInInspector] public float currentBuffPercent;
    [HideInInspector] public int currentMaxStacks;
    [HideInInspector] public MoveSpeedCounter currentCounter;


    public override void Initialize()
    {
        base.Initialize();

        currentBuffPercent = buffPercent;
        currentMaxStacks = maxStacks;

        currentCounter = new MoveSpeedCounter(counter);

        startingAbility = stagePlayerData.startingAbility as DamageAbilityBase;

        player = GameObject.FindGameObjectWithTag("Player");
        AddAndLoadComponent(player);

    }

    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.AddComponent<Deo>();
        objectToAdd.GetComponent<Deo>().LoadData(this);
    }

    public override void UpgradeAbility()
    {
        
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
