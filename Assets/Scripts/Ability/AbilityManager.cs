using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage the player's abilities
public class AbilityManager : MonoBehaviour
{
    private GameObject player; // The player MUST have this script
    public List<AbilityBase> abilities; // List of abilities
    private List<List<GameObject>> bulletPools = new List<List<GameObject>>(); // Each ability has its own bullet pool, each pool contains bullets

    private void Awake()
    {
        player = gameObject;
        foreach (AbilityBase ability in abilities)
        {
            List<GameObject> newBulletPool = ability.Initialize(player);
            bulletPools.Add(newBulletPool);
        }
    }

    // Add an ability to the abilities list
    // Also add a Bullet Pool to the bulletPool list
    public void AddAbility(AbilityBase ability)
    {
        List<GameObject> newBulletPool = ability.Initialize(player);
        bulletPools.Add(newBulletPool);
    }
    
    private void Update()
    {
        // Loop through list of ability, if ability is ready, trigger it
        // The internal state (COOLDOWN, ACTIVE) of the ability is handled through the bullet/ ability script
        // This handle the cooldown, active time AND THE READY STATE of the ability
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
}
