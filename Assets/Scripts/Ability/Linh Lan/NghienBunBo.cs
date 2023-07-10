using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class NghienBunBo : MonoBehaviour
{
    private NghienBunBoData baseData;
    private AbilityCollection currentAbilities;
    private List<DamageAbilityBase> damageAbilities = new List<DamageAbilityBase>();
    
    // Data
    private int damageIncrease;
    private float duration;
    private float internalDuration;

    // Act as state management system
    private bool havingBuff = false;
    private bool justReceivedBuff = false;
    
    // For UI Stuff
    private PassiveAbilityGameEvent activeCountdownImage; // Setup in UIManager.cs
    
    
    private void Update()
    {
        if (havingBuff)
        {
            // If we receive buff while buff still exists
            if (justReceivedBuff)
            {
                // Reset buff timer
                internalDuration = 0f;
                // Immediately reset this or internalDuration will forever be 0
                justReceivedBuff = false;
            }
            internalDuration += Time.deltaTime;
            // Ran out of buff time
            if (internalDuration >= duration)
            {
                internalDuration = 0f;
                foreach (DamageAbilityBase ability in damageAbilities)
                {
                    ability.ModifyDamage(damageIncrease, false);
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
            if (!havingBuff)
            {
                // Loop through every Damage abilities in available Abilities
                foreach (AbilityBase ability in currentAbilities)
                {
                    if (ability is DamageAbilityBase damageAbilityBase)
                    {
                        damageAbilities.Add(damageAbilityBase);
                        damageAbilityBase.ModifyDamage(damageIncrease, true);
                    }
                }
                havingBuff = true;
            }
            // Change state to justReceivedBuff
            justReceivedBuff = true;
            // Active the UI 
            activeCountdownImage.Raise(new PassiveAbilityInfo(duration, baseData.UISprite));
        }
    }

    public void LoadData(NghienBunBoData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }

        currentAbilities = data.currentAbilities;
        activeCountdownImage = data.activeCountdownImage;
        // Things can change during runtime
        damageIncrease = data.currentDamageIncrease;
        duration = data.currentDuration;
    }

}
