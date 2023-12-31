using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// Khi nhận sát thương sẽ có x% cơ hội hồi x% máu dựa trên sát thương nhận vào
/// </summary>
/// 
[CreateAssetMenu(menuName = "Ability/Common/Protection/Fancall")]
public class FancallData : ProtectionAbilityBase<bool>
{
    [Header("Fancall")]
    private GameObject player;
    private PlayerCombat combatScript;
    [Range(0f, 1f), SerializeField] private float healChance;
    [Range(0f, 1f), SerializeField] private float healPercent; 
    [SerializeField] private IntGameEvent healPlayer;

    [Header("Upgrades")]
    [SerializeField] private List<FancallData> upgradeDatas;
    private float currentHealChance;
    private float currentHealPercent;

    public override void Initialize()
    {
        base.Initialize();

        state = AbilityState.active;

        currentHealChance = healChance;
        currentHealPercent = healPercent;

        player = GameObject.FindGameObjectWithTag("Player");
        combatScript = player.GetComponent<PlayerCombat>();
        AddAction();
    }

    public override bool Action(int damage)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < currentHealChance)
        {
            Buff(damage);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Buff(int damage)
    {
        int healAmount = Mathf.RoundToInt(currentHealPercent * damage);
        healPlayer.Raise(healAmount);
    }

    public override void AddAction()
    {
        combatScript.healActions.Add(Action);
    }

    public override void UpgradeAbility()
    {
        FancallData upgradeData = upgradeDatas[currentLevel];

        currentHealChance = upgradeData.healChance;
        currentHealPercent = upgradeData.healPercent;

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
