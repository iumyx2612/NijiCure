using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ForeverAlone : MonoBehaviour
{
    private PlayerCounter counterScript;
    [SerializeField] private AbilityCollection currentAbilities;
    private float damageBuff;
    private float radius;
    private MoveSpeedCounter counter;

    private CircleCollider2D selfCollider;

    private List<Collider2D> enemies = new List<Collider2D>();
    
    // State management
    private bool isAlone;
    private bool havingBuff;

    // Ability buff
    /// <summary>
    /// We have to check if current dmg abilities is changed while this FA is activated 
    /// I.e. adding new ability when FA is activated
    /// 
    /// </summary>
    private List<DamageAbilityBase> currentDmgAbilities = new List<DamageAbilityBase>();
    private List<DamageAbilityBase> prevDmgAbilities = new List<DamageAbilityBase>();
    private float checkTimer;
    private float internalCheckTimer;
    private bool changed = false;


    private void Awake()
    {
        selfCollider = GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
    }

    private void Update()
    {
        // Perform check every check interval
        internalCheckTimer += Time.deltaTime;
        if (internalCheckTimer >= checkTimer)
        {
            changed = IfNumAbilityChanged();
            internalCheckTimer = 0f;
        }

        // Cancel buff (only need to cancel buff in prev)
        if (enemies.Count > 0)
        {
            isAlone = false;
            // Remove buff
            if (havingBuff)
            {
                for (int i = 0; i < prevDmgAbilities.Count; i++)
                {
                    prevDmgAbilities[i].ModifyDamage(damageBuff, false);
                }
            }
            havingBuff = false;
            // Place slow counter
            counterScript.AddMoveSpdCounter(counter);
        }
        else
        {
            isAlone = true;
        }
        // Buff
        if (!havingBuff && isAlone)
        {
            for (int i = 0; i < prevDmgAbilities.Count; i++)
            {
                prevDmgAbilities[i].ModifyDamage(damageBuff, true);
            }

            havingBuff = true;
            // Remove slow counter
            counterScript.RemoveMoveSpdCounter(counter.counterName);
        }
        
        // If having buff and changed:
        // 1. Reduce buff in prev
        // 2. Rebuff in current
        // 3. prev = current
        if (changed)
        {
            if (havingBuff)
            {
                for (int i = 0; i < prevDmgAbilities.Count; i++)
                {
                    prevDmgAbilities[i].ModifyDamage(damageBuff, false);
                }
                for (int i = 0; i < currentDmgAbilities.Count; i++)
                {
                    currentDmgAbilities[i].ModifyDamage(damageBuff, true);
                }
            }
            prevDmgAbilities = new List<DamageAbilityBase>(currentDmgAbilities); // This line needs to be checked
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other);
        }
    }

    private bool IfNumAbilityChanged()
    {
        currentDmgAbilities.Clear();
        for (int i = 0; i < currentAbilities.Count; i++)
        {
            if (currentAbilities[i] is DamageAbilityBase dmgAbility)
            {
                currentDmgAbilities.Add(dmgAbility);
            }
        }
        if (currentDmgAbilities.Count != prevDmgAbilities.Count)
        {
            return true;
        }
        return false;
    }

    public void LoadData(ForeverAloneData data)
    {
        if (counterScript == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            counterScript = player.GetComponent<PlayerCounter>();
            // Fill the prev Abilities List
            for (int i = 0; i < currentAbilities.Count; i++)
            {
                if (currentAbilities[i] is DamageAbilityBase dmgAbility)
                {
                    prevDmgAbilities.Add(dmgAbility);
                }
            }
        }
        damageBuff = data.currentDmgBuff;
        radius = data.currentRadius;
        selfCollider.radius = radius;
        counter = data.currentCounter;
    }
}
