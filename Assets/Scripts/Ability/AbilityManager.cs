using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

// Manage the player's abilities
public class AbilityManager : MonoBehaviour
{
    [Header("Setup all Abilities and Pick-able Abilites")]
    [SerializeField] private AbilityCollection playerAbilities; // All PlayerType Abilities in the game
    [SerializeField] private AbilityCollection allPassives; // All Common Passives
    [SerializeField] private AbilityCollection allDamages; // All Common Damages
    [SerializeField] private AbilityCollection allStatsBuff; // All Stats Buff
    [SerializeField] private AbilityCollection allProtections; // All Common Protection 
    private List<AbilityBase> availableAbilities = new List<AbilityBase>(); // What Ability can appear in the Scene

    [Header("First and current Abilities")]
    [SerializeField] private PlayerData stagePlayerData; // Mapping of PlayerType and Starting Ability
    [SerializeField] private AbilityCollection currentAbilities; // Current abilities that player have 

    [Header("Modify Abilities stuff")]
    [SerializeField] private AbilityGameEvent modifyAbility; // Let player changes ability (Raised in UIManager.cs)
    [SerializeField] private GameEvent levelUpSetUpAbility; // Show 4 Abilities when Level Up (Raised in GameManager.cs)
    [SerializeField] private AbilityCollection abilitiesToPick; // List of 4 Abilities to pick when LvlUp
    [SerializeField] private GameEvent levelUpAbilityUIPopUp; // Setup in UIManager.cs
    [SerializeField] private AbilityDistribution _abilityDistribution; // Act as a reference to abilityDistribution
    [SerializeField] private AbilityGameEvent updateAbilityPanel; // Setup in UIManager.cs
    [SerializeField] private AbilityGameEvent updateAbilityInfo;  // Setup in UIManager.cs
    private AbilityDistribution abilityDistribution;
        
    
    private void Awake()
    {
        // Setup ScriptableObjects Variables and Game Events 
        currentAbilities.Clear();
        modifyAbility.AddListener(ModifyAbility);
        levelUpSetUpAbility.AddListener(SetupAbilitiesWhenLvlUp);

        // Which Ability can be picked in the scene
        PlayerType runtimePlayerType = stagePlayerData.type;
        foreach (AbilityBase ability in playerAbilities)
        {
            if (ability.playerType == runtimePlayerType)
            {
                ability.PreInit();
                if (ability is DamageAbilityBase _damageAbility)
                {
                    _damageAbility.SetupCritChance(stagePlayerData.critChance);
                }
                availableAbilities.Add(ability);
            }
        }
        for (int i = 0; i < allPassives.Count; i++)
        {
            AbilityBase passiveAbility = allPassives[i];
            passiveAbility.PreInit();
            availableAbilities.Add(passiveAbility);
        }
        for (int i = 0; i < allDamages.Count; i++)
        {
            DamageAbilityBase damageAbility = allDamages[i] as DamageAbilityBase;
            damageAbility.PreInit();
            damageAbility.SetupCritChance(stagePlayerData.critChance);
            availableAbilities.Add(damageAbility);
        }
        for (int i = 0; i < allStatsBuff.Count; i++)
        {
            AbilityBase statBuff = allStatsBuff[i];
            statBuff.PreInit();
            availableAbilities.Add(statBuff);
        }
        for (int i = 0; i < allProtections.Count; i++)
        {
            AbilityBase protectionAbility = allProtections[i];
            protectionAbility.PreInit();
            availableAbilities.Add(protectionAbility);
        }
        
        // Set up AbilityDistribution
        abilityDistribution = gameObject.GetComponent<AbilityDistribution>();
        foreach (AbilityBase ability in availableAbilities)
        {
            abilityDistribution.Add(ability, ability.weight);
        }
        _abilityDistribution.SetItems(abilityDistribution.items);
    }

    private void Start()
    {
        currentAbilities.Add(stagePlayerData.startingAbility);
        currentAbilities[0].Initialize();
        updateAbilityPanel.Raise(stagePlayerData.startingAbility);
        updateAbilityInfo.Raise(stagePlayerData.startingAbility);
    }

    private void OnDisable()
    {
        modifyAbility.RemoveListener(ModifyAbility);
        levelUpSetUpAbility.RemoveListener(SetupAbilitiesWhenLvlUp);
    }

    private void Update()
    {
        // Loop through list of ability, if ability is ready, trigger it
        // Switching from COOLDOWN to READY state is handled through this function since every ability follows a SINGLE COOLDOWN RULE
        // Switching from READY to COOLDOWN state is handled IN THE BULLET SCRIPT 
        for (int i = 0; i < currentAbilities.Count; i++)
        {
            AbilityBase ability = currentAbilities[i];
            if (ability.state == AbilityBase.AbilityState.ready)
            {
                ability.TriggerAbility();
                ability.state = AbilityBase.AbilityState.active;
            }

            if (ability.state == AbilityBase.AbilityState.cooldown)
            {
                if (ability.internalCooldownTime < ability.currentCooldownTime)
                {
                    ability.internalCooldownTime += Time.deltaTime;  
                }
                else
                {
                    ability.state = AbilityBase.AbilityState.ready;
                    ability.internalCooldownTime = 0;
                }
            }
        }
    }

    // Allow player to add/ increase the level of Ability
    private void ModifyAbility(AbilityBase ability)
    {
        // If player does not have this ability
        if (!currentAbilities.Contains(ability))
        {
            // Create a new BulletPool for this ability and add it to the List
            currentAbilities.Add(ability);
            ability.Initialize();
            // Update the Ability panel
            updateAbilityPanel.Raise(ability);
            updateAbilityInfo.Raise(ability);
        }
        // If player already has this ability
        else
        {
            ability.UpgradeAbility(); // Upgrade 
        }
        // De-Active the ability selection UI
        levelUpAbilityUIPopUp.Raise();

    }

    // Setup Abilities to pick in UI
    // Then activate the UI for picking Ability
    private void SetupAbilitiesWhenLvlUp()
    {
        _abilityDistribution.items.Clear();
        // Init Referenced Distribution
        _abilityDistribution.SetItems(abilityDistribution.items);
        // Check if Abilities meets requirements to be picked 
        // Loop through all Abilities
        for (int i = _abilityDistribution.items.Count - 1; i >= 0; i--)
        {
            AbilityBase ability = _abilityDistribution.items[i].value;
            // Perform the check:
            // 1. If max level
            // 2. If not ready to be init
            if (ability.IsMaxLevel() || !ability.CanBeInit())
            {
                int index = _abilityDistribution.IndexOf(ability);
                _abilityDistribution.RemoveAt(index);
            }
            // Check if Ability is initialized, which player already have
            // Then re-adjust the weight
            if (ability.isInitialized)
            {
                float w = ability.GetUpgradeDataInfo().weight;
                _abilityDistribution.items[i].weight = w;
            }
        }
        abilitiesToPick.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (_abilityDistribution.items.Count > 0)
            {
                AbilityBase ability = _abilityDistribution.Draw();
                int abilityIndex = _abilityDistribution.IndexOf(ability);
                // Remove from the Referenced Distribution so we don't draw it AGAIN!
                _abilityDistribution.RemoveAt(abilityIndex);
                abilitiesToPick.Add(ability);
            }
        }
        // Active the ability selection UI
        levelUpAbilityUIPopUp.Raise();
    }
}
