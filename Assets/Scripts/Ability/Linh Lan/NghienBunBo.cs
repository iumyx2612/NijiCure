using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class NghienBunBo : MonoBehaviour
{
    public NghienBunBoData abilityData;
    private NghienBunBoData baseData;

    [SerializeField] private AbilityCollection currentAbilities;
    private List<DamageAbilityBase> damageAbilities = new List<DamageAbilityBase>();
    
    // Data
    private int damageIncrease;
    private float duration;

    // Act as state management system
    private bool havingBuff = false;
    private bool justReceivedBuff = false;
    
    [SerializeField] private FloatVariable internalDuration; // This will be used by UI
    
    
    private void Update()
    {
        if (havingBuff)
        {
            // If we receive buff while buff still exists
            if (justReceivedBuff)
            {
                // Reset buff timer
                internalDuration.Value = 0f;
                // Immediately reset this or internalDuration will forever be 0
                justReceivedBuff = false;
            }
            internalDuration.Value += Time.deltaTime;
            // Ran out of buff time
            if (internalDuration.Value >= duration)
            {
                internalDuration.Value = 0f;
                foreach (DamageAbilityBase ability in damageAbilities)
                {
                    ability.PartialModify(-damageIncrease);
                }
                havingBuff = false;
                damageAbilities.Clear();
            }
        }
    }

    // Activate when eat Health items
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Heal"))
        {
            // Loop through every Damage abilities in available Abilities
            foreach (AbilityBase ability in currentAbilities)
            {
                if (ability is DamageAbilityBase damageAbilityBase)
                {
                    damageAbilities.Add(damageAbilityBase);
                    damageAbilityBase.PartialModify(damageIncrease);
                }
            }
            // Change state to havingBuff
            justReceivedBuff = true;
            havingBuff = true;
        }
    }

    public void LoadData(NghienBunBoData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }

        abilityData = data;
        // Things can change during runtime
        damageIncrease = data.currentDamageIncrease;
        duration = data.currentDuration;
    }

}
