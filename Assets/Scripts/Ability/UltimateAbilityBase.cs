using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public abstract class UltimateAbilityBase : ScriptableObject
{
    public float cooldown;
    public BoolVariable canUseUltimate;

    public void Initialize() // Will be init by PlayerCombat.cs
    {
        if (canUseUltimate.Value)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            AddAndLoadUltimate(player);
        }
    }

    public abstract void AddAndLoadUltimate(GameObject player);
}
