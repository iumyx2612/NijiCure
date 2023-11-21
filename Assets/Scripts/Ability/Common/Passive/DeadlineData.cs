using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DeadlineData : PassiveAbilityBase
{
    [Range(0f, 1f)] public float buffPercent;
    public int expGain;
    [Range(0f, 1f)] public float damageBuff;
    public float buffTime;
    public int damageTaken;
    public int healWhenLvlUp;
    public PlayerData stagePlayerData;
    public IntGameEvent increaseExp;
    public IntGameEvent playerTakeDamage;
    public IntGameEvent healPlayer;
    public GameEvent increaseLevel;
    public GameObjectCollection expPickUpPool;

    private DamageAbilityBase startingAbility;
    private bool havingBuff = false;

    [HideInInspector] public float currentBuffPercent;
    [HideInInspector] public int currentExpGain;
    [HideInInspector] public float currentDmgBuff;
    [HideInInspector] public int currentDmgTaken;

    public List<DeadlineData> upgradeDatas;

    public override void Initialize()
    {
        base.Initialize();
        currentBuffPercent = buffPercent;
        currentExpGain = expGain;
        currentDmgBuff = damageBuff;
        currentDmgTaken = damageTaken;
        currentCooldownTime = buffTime;

        startingAbility = stagePlayerData.startingAbility as DamageAbilityBase;
        increaseLevel.AddListener(HealWhenLvlUp);

        // This is insanely time-consuming
        // TODO: Optimize
        for (int i = 0; i < expPickUpPool.Count; i++)
        {
            ExpPickUp expPickUp = expPickUpPool[i].GetComponent<ExpPickUp>();
            expPickUp.afterPickUpActions.Add(Trigger);
        }
    }

    private void HealWhenLvlUp()
    {
        healPlayer.Raise(healWhenLvlUp);
    }

    public void Trigger()
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= currentBuffPercent)
        {
            increaseExp.Raise(currentExpGain);
            if (!havingBuff)
                startingAbility.ModifyDamage(currentDmgBuff, true);
            playerTakeDamage.Raise(currentDmgTaken);
            state = AbilityState.cooldown;
            internalCooldownTime = 0f;
            havingBuff = true;
        }
    }

    public override void TriggerAbility()
    {
        havingBuff = false;
        startingAbility.ModifyDamage(currentDmgBuff, false);   
    }

    public override void AddAndLoadComponent(GameObject objectToAdd) {}

    public override void UpgradeAbility()
    {
        base.UpgradeAbility();
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
