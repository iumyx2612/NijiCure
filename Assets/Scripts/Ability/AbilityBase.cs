using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    public string name;
    public Sprite UISprite;
    public float baseCooldownTime;
    [HideInInspector] public int currentLevel; // For upgrade purpose
    [HideInInspector] public float currentCooldownTime; // For upgrade purpose
    [HideInInspector] public float internalCooldownTime; // This increment every update function to meet cooldownTime (if ability has cooldown)
    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    [HideInInspector] public AbilityState state; // Set in initialize
    
    // Create a BulletPool
    public abstract List<GameObject> Initialize(GameObject player);

    // Trigger the ability
    // Every ability has its own BulletPool
    public abstract void TriggerAbility(List<GameObject> bulletPool);

    // Apply upgrade on to the entire Pool
    public abstract void UpgradeAbility(List<GameObject> bulletPool, int tier);
}