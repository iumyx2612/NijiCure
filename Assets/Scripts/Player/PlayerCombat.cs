using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


public struct TypeAndBaseAbility
{
    public PlayerType playerType;
    public AbilityBase startingAbility;
}


public class PlayerCombat : MonoBehaviour
{
    public PlayerData playerData;
    [SerializeField] private PlayerTypeAndStartingAbility mapping;
    
    // Data
    private int health;
    private int rank;
    private float critChance;
    
    [SerializeField] private BoolVariable isAlive;
    [SerializeField] private GameEvent onPlayerKilled;
    [SerializeField] private IntGameEvent playerTakeDamage;
    

    private void Awake()
    {
        // Set up variables and stuff
        onPlayerKilled.AddListener(Dead);
        playerTakeDamage.AddListener(TakeDamage);
        mapping.playerType = playerData.type;
        mapping.startingAbility = playerData.startingAbility;
    }

    private void OnEnable()
    {
        isAlive.Value = true;
        if (playerData != null)
        {
            LoadData(playerData);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
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
        health = data.health;
        rank = data.rank;
    }
}
