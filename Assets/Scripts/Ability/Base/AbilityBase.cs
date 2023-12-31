using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    [Header("Base")]
    public string abilityName;
    public Sprite abilityIcon;
    public Sprite typeIcon;
    public float cooldownTime;
    public string description;
    public string summary;
    public string debuffDesc; // Only use in starting Ability
    [HideInInspector] public int currentLevel; // Setup in AbilityManager.cs
    [HideInInspector] public float currentCooldownTime;
    [HideInInspector] public float internalCooldownTime; // This increment every update function to meet cooldownTime (if ability has cooldown)
    
    public PlayerType playerType;
    public float weight; // Chances to appear when leveled up
    [HideInInspector] public bool isInitialized;
    
    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    public AbilityState state; // Set in initialize

    public virtual void PreInit() 
    {
        currentLevel = 0;
        isInitialized = false;
        internalCooldownTime = 0f;
    }
    
    // Setup the Ability
    public abstract void Initialize();

    // Trigger the ability
    public abstract void TriggerAbility();

    // Apply upgrade on to the entire Ability
    public abstract void UpgradeAbility();

    // If an Ability is dependent on another Ability 
    // Then it MUST re-implement this method
    public abstract bool CanBeInit();

    // -------------------------------------------------------
    // Access the generic info of UpgradeData like description, name, UISprite
    // THIS IS USED ONLY IN UIManager.cs TO DISPLAY THE UPGRADE INFO
    public abstract AbilityBase GetUpgradeDataInfo();

    public abstract bool IsMaxLevel();
    // -------------------------------------------------------
    
    // -------------------------------------------------------
    public abstract void ModifyDebuff(int level);
}
