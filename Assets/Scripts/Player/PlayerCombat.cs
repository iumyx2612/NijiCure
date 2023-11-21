using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData;    
    // Data
    [SerializeField] private IntVariable playerBaseHealth;
    [SerializeField] private IntVariable playerCurrentHealth;
    private int rank;
    private UltimateAbilityBase ultimateAbility;

    [SerializeField] private BoolVariable isAlive;
    [SerializeField] private GameEvent onPlayerKilled;
    [SerializeField] private IntGameEvent playerProcessDamage;
    [SerializeField] private IntGameEvent playerTakeDamage;
    
    [SerializeField] private GameEvent healthBarImageUpdate;
    [SerializeField] private IntGameEvent healthTextPopupGameEvent;
    [SerializeField] private IntGameEvent healPlayer;

    // Combat Handling stuff
    /// <summary>
    /// Combat flow:
    /// - Attack signal -> Attack signal handling:
    ///     + Dodge -> Shield -> Go through -> Attack dmg handling:
    ///         + Heal -> Reduce dmg -> Full dmg
    /// - Dodge doesn't stack -> Can have multiple dodge abilites with separate chances
    /// - Shield -> Player can only pick 1 shielding ability, if one is picked then others are removed 
    /// - Heal doesn't stack -> Heal multiple times
    /// - If all heal fails -> Reduce dmg, same with shield
    /// </summary>
    [HideInInspector] public readonly List<Func<int, bool>> dodgeActions = new List<Func<int, bool>>(); 
    [HideInInspector] public readonly List<Func<int, int>> shieldActions = new List<Func<int, int>>(); // This is a list but its length is fixed to 1
    [HideInInspector] public readonly List<Func<int, bool>> healActions = new List<Func<int, bool>>();
    [HideInInspector] public readonly List<Func<int, int>> reduceDmgActions = new List<Func<int, int>>(); // This is a list but its length is fixed to 1

    // Counter
    [HideInInspector] public List<DamageBuffCounter> dmgBuffCounters = new List<DamageBuffCounter>();
    private float counterDmgMultiplier;


    private void Awake()
    {
        // Avoid error while testing
        if (playerData != null)
        {
            LoadData(playerData);
        }
        // Set up variables and stuff
        onPlayerKilled.AddListener(Dead);
        playerProcessDamage.AddListener(AttackSignalHandler);
        playerTakeDamage.AddListener(TakeDamage);
        healPlayer.AddListener(HealPlayer);
    }

    private void OnEnable()
    {
        isAlive.Value = true;
        playerCurrentHealth.Value = playerBaseHealth.Value;
    }

    private void OnDisable()
    {
        onPlayerKilled.RemoveListener(Dead);
        playerProcessDamage.RemoveListener(AttackSignalHandler);
        playerTakeDamage.RemoveListener(TakeDamage);
        healPlayer.RemoveListener(HealPlayer);
    }

    // ------------------ Combat Flow ------------------
    private void AttackSignalHandler(int damage)
    {
        bool dodged = false;
        for (int i = 0; i < dodgeActions.Count; i++)
        {
            bool result = dodgeActions[i](damage);
            if (result == true)
            {
                dodged = true;
                break;
            }
        }
        int dmgAfterShielded = damage;
        if (!dodged)
        {
            for (int i = 0; i < shieldActions.Count; i++)
            {
                dmgAfterShielded = shieldActions[i](damage);
            }
        }
        if (dmgAfterShielded > 0 && !dodged)
        {
            AttackDamageHandler(damage);
        }
    }

    private void AttackDamageHandler(int damage)
    {
        bool hasHealed = false;
        for (int i = 0; i < healActions.Count; i++)
        {
            hasHealed = healActions[i](damage);
        }
        int dmgAfterReduced = damage;
        if (!hasHealed)
        {
            for (int i = 0; i < reduceDmgActions.Count; i++)
            {
                dmgAfterReduced = reduceDmgActions[i](damage);
            }
        }
        if (dmgAfterReduced > 0)
        {
            playerTakeDamage.Raise(dmgAfterReduced);
        }
    }

    private void TakeDamage(int damage)
    {
        counterDmgMultiplier = 1f;
        for (int i = 0; i < dmgBuffCounters.Count; i++)
        {
            DamageBuffCounter counter = dmgBuffCounters[i];
            counterDmgMultiplier += counter.damageBuff;
        }
        int actualDamage = Mathf.RoundToInt(damage * counterDmgMultiplier);
        playerCurrentHealth.Value -= actualDamage;
        healthBarImageUpdate.Raise(); // Check PlayerUIManager.cs
        healthTextPopupGameEvent.Raise(-actualDamage); // Check PlayerUIManager.cs
        if (playerCurrentHealth.Value <= 0)
        {
            onPlayerKilled.Raise();
        }
    }

    private void TakeDamageFromAbility(int damage)
    {
        playerCurrentHealth.Value -= damage;
        healthBarImageUpdate.Raise(); // Check PlayerUIManager.cs
    }

    private void Dead()
    {
        isAlive.Value = false;
        gameObject.SetActive(false);
    }
    
    private void LoadData(PlayerData data)
    {
        playerData = data;
        playerBaseHealth.Value = data.health;
        rank = data.rank;
        ultimateAbility = data.ultimateAbility;
        ultimateAbility.Initialize(gameObject);
    }

    private void HealPlayer(int healAmount)
    {
        playerCurrentHealth.Value += healAmount;
        if (playerCurrentHealth.Value >= playerBaseHealth.Value)
        {
            playerCurrentHealth.Value = playerBaseHealth.Value;
        }
        healthBarImageUpdate.Raise(); // Check PlayerUIManager.cs
        healthTextPopupGameEvent.Raise(healAmount); // Check PlayerUIManager.cs
    }

}
