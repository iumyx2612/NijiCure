using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    public string abilityName;
    public Sprite UISprite;
    public float cooldownTime;
    public string description;
    [HideInInspector] public int currentLevel; // Setup in AbilityManager.cs
    [HideInInspector] public float currentCooldownTime;
    [HideInInspector] public float internalCooldownTime; // This increment every update function to meet cooldownTime (if ability has cooldown)
    
    public PlayerType playerType;
    public float weight; // Chances to appear when leveled up
    
    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    public AbilityState state; // Set in initialize
    
    // Setup the Ability
    public abstract void Initialize();

    // Trigger the ability
    public abstract void TriggerAbility();

    // Apply upgrade on to the entire Ability
    public abstract void UpgradeAbility();

    // Access the generic info of UpgradeData like description, name, UISprite
    // THIS IS USED ONLY IN UIManager.cs TO DISPLAY THE UPGRADE INFO
    public abstract AbilityBase GetUpgradeDataInfo();

    public abstract void PartialModify(int value);

    public abstract bool IsMaxLevel();
}
