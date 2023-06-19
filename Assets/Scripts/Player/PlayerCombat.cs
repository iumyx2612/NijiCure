using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData;
    [SerializeField] private PlayerTypeAndStartingAbility mapping; // Reference by others' 
    
    // Data
    [SerializeField] private IntVariable playerBaseHealth;
    [SerializeField] private IntVariable playerCurrentHealth;
    private int rank;
    private float critChance;
    private UltimateAbilityBase ultimateAbility;
    
    [SerializeField] private BoolVariable isAlive;
    [SerializeField] private GameEvent onPlayerKilled;
    [SerializeField] private IntGameEvent playerTakeDamage;
    
    [SerializeField] private Vector2GameEvent healthBarImageUpdate;
    [SerializeField] private IntGameEvent healthTextPopupGameEvent;
    [SerializeField] private IntGameEvent healPlayer;

    
    private void Awake()
    {
        // Set up variables and stuff
        onPlayerKilled.AddListener(Dead);
        playerTakeDamage.AddListener(TakeDamage);
        healPlayer.AddListener(HealPlayer);
        mapping.playerType = playerData.type;
        mapping.startingAbility = playerData.startingAbility;
    }

    private void OnEnable()
    {
        isAlive.Value = true;
        if (playerData != null)
        {
            LoadData(playerData);
            playerCurrentHealth.Value = playerBaseHealth.Value;
        }
    }

    private void OnDisable()
    {
        onPlayerKilled.RemoveListener(Dead);
        playerTakeDamage.RemoveListener(TakeDamage);
        healPlayer.RemoveListener(HealPlayer);
    }

    private void TakeDamage(int damage)
    {
        playerCurrentHealth.Value -= damage;
        healthBarImageUpdate.Raise(transform.position); // Check PlayerUIManager.cs
        healthTextPopupGameEvent.Raise(-damage); // Check PlayerUIManager.cs
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
    
    public void LoadData(PlayerData data)
    {
        playerData = data;
        playerBaseHealth.Value = data.health;
        rank = data.rank;
        ultimateAbility = data.ultimateAbility;
        ultimateAbility.Initialize();
    }

    private void HealPlayer(int healAmount)
    {
        playerCurrentHealth.Value += healAmount;
        if (playerCurrentHealth.Value >= playerBaseHealth.Value)
        {
            playerCurrentHealth.Value = playerBaseHealth.Value;
        }
        healthBarImageUpdate.Raise(transform.position); // Check PlayerUIManager.cs
        healthTextPopupGameEvent.Raise(healAmount); // Check PlayerUIManager.cs
    }
}