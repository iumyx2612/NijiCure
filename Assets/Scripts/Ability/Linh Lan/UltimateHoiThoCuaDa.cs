using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


public class UltimateHoiThoCuaDa : MonoBehaviour
{
    public UltimateHoiThoCuaDaData ultimateData;

    private AbilityCollection currentAbilites;
    
    // Data
    private float activeTime;
    private float cooldown;

    private float internalActiveTime;
    private float internalCooldown;
    
    // States
    private enum State
    {
        ready,
        active,
        cooldown
    }

    private State state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.ready;
    }

    // Handle states of Ultimate
    // Also handle how the Ultimate interact
    void Update()
    {
        if (state == State.ready && Input.GetKeyDown(KeyCode.X))
        {
            state = State.active;
            TriggerUltimate(true);
        }
        
        else if (state == State.active)
        {
            internalActiveTime += Time.deltaTime;
            if (internalActiveTime >= activeTime)
            {
                TriggerUltimate(false);
                internalActiveTime = 0f;
                state = State.cooldown;
            }
        }

        else if (state == State.cooldown)
        {
            internalCooldown += Time.deltaTime;
            if (internalCooldown >= cooldown)
            {
                internalCooldown = 0f;
                state = State.ready;
            }
        }
    }

    public void LoadData(UltimateHoiThoCuaDaData data)
    {
        ultimateData = data;
        activeTime = data.activeTime;
        cooldown = data.cooldown;
        currentAbilites = data.currentAbilites;
    }

    private void TriggerUltimate(bool upgrade)
    {
        // Get the LanKnife ability
        LanKnifeData lanKnifeAbility = null;
        foreach (AbilityBase ability in currentAbilites)
        {
            if (ability is LanKnifeData lanKnife)
            {
                lanKnifeAbility = lanKnife;
                break;
            }
        }

        if (upgrade)
        {
            // Apply effects to LanKnife
            LanKnifeMultiplier(lanKnifeAbility);   
        }
        else
        {
            LanKnifeDivider(lanKnifeAbility);
        }
        
    }

    private void LanKnifeMultiplier(LanKnifeData ability)
    {
        ability.currentDamage = Mathf.RoundToInt(ability.currentDamage * ultimateData.damageMultiplier);
        ability.currentCooldownTime *= ultimateData.cooldownReduction;
        ability.currentKnifeDistance *= ultimateData.knifeDistanceMultiplier;
        ability.currentKnifeScale *= ultimateData.knifeScaleMultiplier;
        ability.currentKnifeSpeed *= ultimateData.knifeSpeedMultiplier;
        GameObject bullet = ability.pool[0];
        bullet.GetComponent<LanKnife>().LoadData(ability);
        Debug.Log("Upgraded");
    }

    private void LanKnifeDivider(LanKnifeData ability)
    {
        ability.currentDamage = Mathf.RoundToInt(ability.currentDamage / ultimateData.damageMultiplier);
        ability.currentCooldownTime /= ultimateData.cooldownReduction;
        ability.currentKnifeDistance /= ultimateData.knifeDistanceMultiplier;
        ability.currentKnifeScale /= ultimateData.knifeScaleMultiplier;
        ability.currentKnifeSpeed /= ultimateData.knifeSpeedMultiplier;
        GameObject bullet = ability.pool[0];
        bullet.GetComponent<LanKnife>().LoadData(ability);
        Debug.Log("End");
    }
}
