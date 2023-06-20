using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

// Manage the player's abilities
public class AbilityManager : MonoBehaviour
{
    [Header("Setup all Abilities and Pick-able Abilites")]
    [SerializeField] private AbilityCollection allAbilities; // All created Abilities in the game
    [SerializeField] private PlayerType typeAny; // For adding Ability 
    private List<AbilityBase> availableAbilities = new List<AbilityBase>(); // What Ability can appear in the Scene

    [Header("First and current Abilities")]
    [SerializeField] private PlayerTypeAndStartingAbility mapping; // Mapping of PlayerType and Starting Ability
    [SerializeField] private AbilityCollection currentAbilities; // Current abilities that player have 

    [Header("Modify Abilities stuff")]
    [SerializeField] private AbilityGameEvent modifyAbility; // Let player changes ability (Raised in UIManager.cs)
    [SerializeField] private GameEvent levelUpSetUpAbility; // Show 4 Abilities when Level Up (Raised in GameManager.cs)
    [SerializeField] private AbilityCollection abilitiesToPick; // List of 4 Abilities to pick when LvlUp
    [SerializeField] private GameEvent levelUpAbilityUIPopUp; // Setup in UIManager.cs
    [SerializeField] private AbilityDistribution _abilityDistribution; // Act as a reference to abilityDistribution
    private AbilityDistribution abilityDistribution;
    
    
    private void Awake()
    {
        // Setup ScriptableObjects Variables and Game Events 
        currentAbilities.Clear();
        modifyAbility.AddListener(ModifyAbility);
        levelUpSetUpAbility.AddListener(SetupAbilitiesWhenLvlUp);

        // Which Ability can be picked in the scene
        PlayerType runtimePlayerType = mapping.playerType;
        foreach (AbilityBase ability in allAbilities)
        {
            if (ability.playerType == runtimePlayerType || ability.playerType == typeAny)
            {
                ability.currentLevel = 0;
                availableAbilities.Add(ability);
            }
        }
        // Set up AbilityDistribution
        abilityDistribution = gameObject.GetComponent<AbilityDistribution>();
        foreach (AbilityBase ability in availableAbilities)
        {
            abilityDistribution.Add(ability, ability.weight);
        }
    }

    private void Start()
    {
        currentAbilities.Add(mapping.startingAbility);
        currentAbilities[0].Initialize();
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
        // Init Referenced Distribution
        _abilityDistribution.SetItems(abilityDistribution.Items);
        // Check if any Abilities has reach its max level
        for (int i = _abilityDistribution.Items.Count - 1; i >= 0; i--)
        {
            AbilityBase ability = _abilityDistribution.Items[i].Value;
            if (ability.IsMaxLevel())
            {
                int index = _abilityDistribution.IndexOf(ability);
                _abilityDistribution.RemoveAt(index);
            }
        }
        abilitiesToPick.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (_abilityDistribution.Items.Count > 0)
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
