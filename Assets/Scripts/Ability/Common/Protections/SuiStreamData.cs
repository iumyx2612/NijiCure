using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// Có x% cơ hội né 1 đòn
/// Khi né thành công sẽ buff x% dmg cho kĩ năng cơ bản trong x(s)
/// </summary>
[CreateAssetMenu(menuName = "Ability/Common/Protection/SuiStream")]
public class SuiStreamData : ProtectionAbilityBase<bool>
{
    private GameObject player;
    private PlayerCombat combatScript;
    public PlayerData stagePlayerData;
    private DamageAbilityBase startingAbility;
    [Range(0f, 1f)] public float dodgeChance;
    [Range(0f, 1f)] public float dmgBuff;
    public float dmgBuffTime;
    [Header("Visual stuff")]
    public PassiveAbilityGameEvent activeCountdownImage; // Setup in UIManager.cs
    public TextNColorGameEvent statusTextPopUpGameEvent; // Setup in PlayerUIManager.cs
    public TextNColor popupConfig;

    [HideInInspector] public float currentDodgeChance;
    [HideInInspector] public float currentDmgBuff;

    [Header("Upgrades")]
    public List<SuiStreamData> upgradeDatas;

    private bool havingBuff = false;


    public override void Initialize()
    {
        base.Initialize();
        currentDodgeChance = dodgeChance;
        currentDmgBuff = dmgBuff;
        currentCooldownTime = dmgBuffTime;

        player = GameObject.FindGameObjectWithTag("Player");
        combatScript = player.GetComponent<PlayerCombat>();
        startingAbility = stagePlayerData.startingAbility as DamageAbilityBase;
        AddAction();
    }
    public override bool Action(int damage)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < currentDodgeChance)
        {
            Buff();
            statusTextPopUpGameEvent.Raise(popupConfig);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Buff()
    {
        if (!havingBuff)
            startingAbility.ModifyDamage(currentDmgBuff, true);
        state = AbilityState.cooldown;
        internalCooldownTime = 0f;
        activeCountdownImage.Raise(new PassiveAbilityInfo(currentCooldownTime, abilityIcon, 0, false, false));
        havingBuff = true;
    }
    public override void TriggerAbility()
    {
        havingBuff = false;
        startingAbility.ModifyDamage(currentDmgBuff, false);
    }

    public override void AddAction()
    {
        combatScript.dodgeActions.Add(Action);
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

    public override void UpgradeAbility()
    {
        SuiStreamData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentDodgeChance = upgradeData.dodgeChance;
        currentCooldownTime = upgradeData.dmgBuffTime;
        currentDmgBuff = upgradeData.dmgBuff;

        currentLevel += 1;        
    }
}
