using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    // Health stuff (Will be moved into ScriptableObject Data)
    [SerializeField] private IntVariable basePlayerHealth; 
    [SerializeField] private IntVariable currentPlayerHealth; // Track the player's health in the scene
    private bool isAlive = true; // The name explains itself
    [SerializeField] private GameEvent onPlayerKill; // Trigger when player dies
    [SerializeField] private IntGameEvent playerTakeDamage; // Trigger when player takes dmg
    
    // Attack stuff (Will be moved into ScriptableObject Data)
    [SerializeField] private FloatVariable timePerAttack;
    [SerializeField] private GameEvent beginAttack; // Trigger when player attacks
    
    // Bullet stuff 
    [SerializeField] private GameObject bulletHolder; // The bullet holder in the scene (keep things in neat)
    private List<GameObject> bulletPooling = new List<GameObject>(); // Bullet Pool
    [SerializeField] private GameObject bulletPrefab; // The bullet
    [SerializeField] private int bulletToSpawn = 2; // For pooling
    
    private void Awake()
    {
        // Instantiate bullet to the Pool
        for (int i = 0; i < bulletToSpawn; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletHolder.transform);
            bulletPooling.Add(bullet);
            bullet.SetActive(false);
        }
    }
    private void Start()
    {
        // Register some Scriptable Events and Variables
        currentPlayerHealth.Value = basePlayerHealth.Value;
        playerTakeDamage.AddListener(TakeDamage);
        onPlayerKill.AddListener(Dead);
        beginAttack.AddListener(Attack);
        StartCoroutine(AttackScheduling());
    }
    
    // This function uses to call the Attack() function 
    // by raising the Attack GameEvent
    IEnumerator AttackScheduling()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(timePerAttack);
            beginAttack.Raise();
        }
    }
    
    // Called by playerTakeDamage
    public void TakeDamage(int damage)
    {
        currentPlayerHealth.Value -= damage;
        if (currentPlayerHealth.Value <= 0)
        {
            onPlayerKill.Raise();
        }
        
    }
    
    // Called by onPlayerKill
    public void Dead()
    {
        isAlive = false;
    }
    
    // Called by beginAttack
    private void Attack()
    {
        foreach (GameObject bullet in bulletPooling)
        {
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
                break;
            }
        }
    }
}
