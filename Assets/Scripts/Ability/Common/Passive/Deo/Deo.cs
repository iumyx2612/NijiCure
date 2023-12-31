using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.AI;

public class Deo : MonoBehaviour
{
    private DeoData baseData;
    private PlayerCounter playerCounterScript;

    private int stackPerSec;
    private float cooldownWhenHit;
    private int maxStacks;
    private float buffPercent;
    private DamageAbilityBase startingAbility;
    private MoveSpeedCounter counter;
    private IntGameEvent playerTakeDamage;
    private PassiveAbilityGameEvent activeCountdownImage;
    private Sprite abilityIcon;

    [SerializeField] private float internalStackTimer;
    [SerializeField] private int currentStacks;
    [SerializeField] private float totalBuffPercent;
    [SerializeField] private bool canHaveBuff;
    [SerializeField] private float internalCooldownTimer;


    private void OnDisable()
    {
        playerTakeDamage.RemoveListener(ResetStacks);
    }

    // Update is called once per frame
    void Update()
    {
        internalStackTimer += Time.deltaTime;
        // Countdown to increase stack
        if (internalStackTimer >= stackPerSec && canHaveBuff)
        {
            if (currentStacks < maxStacks)
            {
                currentStacks += 1;
                // If get buffed for the 1st stack -> Increase dmg
                // Also apply the Slow counter
                if (currentStacks == 1)
                {
                    totalBuffPercent = buffPercent;
                    startingAbility.ModifyDamage(totalBuffPercent, true);
                    playerCounterScript.AddMoveSpdCounter(counter);
                }
                // If get buffet not on the 1st stack 
                // -> Decrease the dmg equals to the last dmg buff
                // -> Recalculate the new dmg buff
                // -> Apply new dmg buff
                else
                {
                    startingAbility.ModifyDamage(totalBuffPercent, false);
                    totalBuffPercent = buffPercent * currentStacks;
                    startingAbility.ModifyDamage(totalBuffPercent, true);
                }
                activeCountdownImage.Raise(
                    new PassiveAbilityInfo(0f, abilityIcon, currentStacks, true, false)
                );
                // Remove counter if max stacks
                if (currentStacks >= maxStacks)
                {
                    playerCounterScript.RemoveMoveSpdCounter(counter.counterName);
                }
            }
            internalStackTimer = 0f;
        }
        if (!canHaveBuff)
        {
            internalCooldownTimer += Time.deltaTime;
            if (internalCooldownTimer >= cooldownWhenHit)
            {
                canHaveBuff = true;
                internalCooldownTimer = 0f;
                internalStackTimer = 0f;
            }
        }
    }

    public void LoadData(DeoData data)
    {
        if (baseData == null)
        {
            baseData = data;
            playerCounterScript = GetComponent<PlayerCounter>();
        }
        if (startingAbility == null)
        {
            startingAbility = data.startingAbility;
        }
        if (playerTakeDamage == null)
        {
            playerTakeDamage = data.playerTakeDamage;
            playerTakeDamage.AddListener(ResetStacks);
        }
        if (activeCountdownImage == null)
        {
            activeCountdownImage = data.activeCountdownImage;
            abilityIcon = data.abilityIcon;
        }

        stackPerSec = data.stackPerSec;
        maxStacks = data.currentMaxStacks;

        if (totalBuffPercent > 0f) // If player is having buff
            startingAbility.ModifyDamage(totalBuffPercent, false); // Remove the buff first
        buffPercent = data.currentBuffPercent; // Buff will be re-apply on next frame in Update

        counter = data.currentCounter;
        cooldownWhenHit = data.cooldownWhenHit;
        

    }

    private void ResetStacks(int damage)
    {
        currentStacks = 0;
        startingAbility.ModifyDamage(totalBuffPercent, false);
        totalBuffPercent = 0f;
        canHaveBuff = false;
        internalCooldownTimer = 0f;
        // Reset UI
        activeCountdownImage.Raise(
            new PassiveAbilityInfo(0f, abilityIcon, currentStacks, true, true) 
        );
        // Reset counters on player
        playerCounterScript.RemoveMoveSpdCounter(counter.counterName);

    }
}
