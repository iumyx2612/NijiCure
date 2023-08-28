using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData;
    [SerializeField] private PlayerMapping mapping; // Reference by others' 
    
    // Data
    [SerializeField] private IntVariable playerBaseHealth;
    [SerializeField] private IntVariable playerCurrentHealth;
    private int rank;
    private UltimateAbilityBase ultimateAbility;
    [HideInInspector] public Action afterDodgeAction;

    [SerializeField] private BoolVariable isAlive;
    [SerializeField] private GameEvent onPlayerKilled;
    [SerializeField] private IntGameEvent playerTakeDamage;
    [SerializeField] private FloatVariable playerDodgeChance;
    
    [SerializeField] private GameEvent healthBarImageUpdate;
    [SerializeField] private IntGameEvent healthTextPopupGameEvent;
    [SerializeField] private IntGameEvent healPlayer;

    
    private void Awake()
    {
        // Avoid error while testing
        if (playerData != null)
        {
            LoadData(playerData);
        }
        // Set up variables and stuff
        onPlayerKilled.AddListener(Dead);
        playerTakeDamage.AddListener(TakeDamage);
        healPlayer.AddListener(HealPlayer);
        mapping.playerType = playerData.type;
        mapping.startingAbility = playerData.startingAbility;
        mapping.critChance = playerData.critChance;
        playerDodgeChance.Value = 0f;
    }

    private void OnEnable()
    {
        isAlive.Value = true;
        playerCurrentHealth.Value = playerBaseHealth.Value;
    }

    private void OnDisable()
    {
        onPlayerKilled.RemoveListener(Dead);
        playerTakeDamage.RemoveListener(TakeDamage);
        healPlayer.RemoveListener(HealPlayer);
    }

    private void TakeDamage(int damage)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < playerDodgeChance.Value)
        {
            afterDodgeAction();
        }
        else
        {
            playerCurrentHealth.Value -= damage;
            healthBarImageUpdate.Raise(); // Check PlayerUIManager.cs
            healthTextPopupGameEvent.Raise(-damage); // Check PlayerUIManager.cs
        }
        if (playerCurrentHealth.Value <= 0)
        {
            onPlayerKilled.Raise();
        }
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
