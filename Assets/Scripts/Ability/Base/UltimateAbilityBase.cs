using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public abstract class UltimateAbilityBase : ScriptableObject
{
    public Sprite UISprite;
    public string ultimateName;
    public string ultimateDescription;
    public float cooldown;
    public BoolVariable canUseUltimate;

    public void Initialize(GameObject player) // Will be init by PlayerCombat.cs
    {
        if (canUseUltimate.Value)
        {
            AddAndLoadUltimate(player);
        }
    }

    public abstract void AddAndLoadUltimate(GameObject player);
}
