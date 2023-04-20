using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    // I set some of the variables to Scriptable Variable in order for easy 
    // balancing in the future
    // Health stuff
    [SerializeField] private IntVariable basePlayerHealth;
    [SerializeField] private IntVariable currentPlayerHealth;
    private bool isAlive = true;
    [SerializeField] private GameEvent onPlayerKill;
    [SerializeField] private IntGameEvent playerTakeDamage;
    
    // Attack stuff
    [SerializeField] private FloatVariable timePerAttack;
    [SerializeField] private GameEvent beginAttack;
    
    // Bullet stuff
    [SerializeField] private GameObject bulletHolder;
    private List<GameObject> bulletPooling = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletToSpawn = 2;
    
    private void Awake()
    {
        for (int i = 0; i < bulletToSpawn; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletHolder.transform);
            bulletPooling.Add(bullet);
            bullet.SetActive(false);
        }
    }
    private void Start()
    {
        currentPlayerHealth.Value = basePlayerHealth.Value;
        playerTakeDamage.AddListener(TakeDamage);
        onPlayerKill.AddListener(Dead);
        beginAttack.AddListener(Attack);
        StartCoroutine(AttackScheduling());
    }
    
    // Update is called once per frame
    void Update()
    {
        
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
    
    public void TakeDamage(int damage)
    {
        currentPlayerHealth.Value -= damage;
        if (currentPlayerHealth.Value <= 0)
        {
            onPlayerKill.Raise();
        }
        
    }

    public void Dead()
    {
        isAlive = false;
    }

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
