using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mỗi x (s) tăng 1 stack Dẹo (max 5), mỗi stack dẹo tăng dmg base ability thêm x%
/// Khi chưa max stack Dẹo thì bị slow x%
/// Khi trúng đòn thì mất toàn bộ stack, phải đợi x (s) thì mới tăng stack
/// </summary>
public class DeoData : PassiveAbilityBase
{
    public int stackPerSec;
    public int maxStacks;
    [Range(0f, 1f)] public float buffPercent;
    [Range(0f, 1f)] public float slowPercent;
    public PlayerData stagePlayerData;
    private DamageAbilityBase startingAbility;
    public GameObject deoPrefab;
    private GameObject deoHolder;

    [HideInInspector] public float currentBuffPercent;
    [HideInInspector] public float currentSlowPercent;
    [HideInInspector] public int currentMaxStacks;


    public override void Initialize()
    {
        base.Initialize();

        currentBuffPercent = buffPercent;
        currentSlowPercent = slowPercent;
        currentMaxStacks = maxStacks;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AddAndLoadComponent(player);

    }

    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        deoHolder = Instantiate(deoPrefab, objectToAdd.transform);
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
