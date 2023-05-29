using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

// Manage the player's abilities
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private PlayerTypeAndStartingAbility mapping; // Mapping of PlayerType and Starting Ability
    [SerializeField] private AbilityCollection abilities; // Current abilities that player have 
    private List<List<GameObject>> bulletPools = new List<List<GameObject>>(); // Each ability has its own bullet pool, each pool contains bullets

    [SerializeField] private AbilityGameEvent modifyAbility; // Let player changes ability (Add or Upgrade)
    [SerializeField] private GameEvent levelUpShowAbility; // Show 4 Abilities when Level Up
    [SerializeField] private AbilityCollection allAbilities; // All created Abilities in the game
    [SerializeField] private PlayerType typeAny; // For adding Ability 
    private AbilityDistribution abilityDistribution;
    private List<AbilityBase> availableAbilities = new List<AbilityBase>();
    private AbilityCollection abilitiesToPick;
    
    private void Awake()
    {
        abilities.Clear();
        abilityDistribution = gameObject.GetComponent<AbilityDistribution>();
        modifyAbility.AddListener(ModifyAbility);
//        levelUpShowAbility.AddListener(ShowAbilityWhenLvlUp);
        
        // Set up which Ability can be picked in the scene
        PlayerType runtimePlayerType = mapping.playerType;
        foreach (AbilityBase ability in allAbilities)
        {
            if (ability.playerType == runtimePlayerType || ability.playerType == typeAny)
            {
                availableAbilities.Add(ability);
            }
        }
        // Set up AbilityDistribution
        foreach (AbilityBase ability in availableAbilities)
        {
            abilityDistribution.Add(ability, ability.weight);
        }
    }

    private void Start()
    {
        abilities.Add(mapping.startingAbility);
        List<GameObject> newBulletPool = abilities[0].Initialize();
        bulletPools.Add(newBulletPool);
    }
    
    private void Update()
    {
        // Loop through list of ability, if ability is ready, trigger it
        // Switching from COOLDOWN to READY state is handled through this function since every ability follows a SINGLE COOLDOWN RULE
        // Switching from READY to COOLDOWN state is handled IN THE BULLET SCRIPT 
        for (int i = 0; i < abilities.Count; i++)
        {
            AbilityBase ability = abilities[i];
            if (ability.state == AbilityBase.AbilityState.ready)
            {
                List<GameObject> bulletPool = bulletPools[i];
                ability.TriggerAbility(bulletPool);
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
    public void ModifyAbility(AbilityBase ability)
    {
        // If player does not have this ability
        if (!abilities.Contains(ability))
        {
            // Create a new BulletPool for this ability and add it to the List
            abilities.Add(ability);
            List<GameObject> newBulletPool = ability.Initialize();
            bulletPools.Add(newBulletPool);
        }
        // If player already has this ability
        else
        {
            int index = abilities.IndexOf(ability); // Find the index of the ability
            ability.UpgradeAbility(bulletPools[index]); // Upgrade the entire BulletPool
        }
    }

    public void ShowAbilityWhenLvlUp()
    {
        // Show 4 Abilities when Level Up
        for (int i = 0; i < 4; i++)
        {
            AbilityBase ability = abilityDistribution.Draw();
            abilitiesToPick.Add(ability);
        }
    }
}
