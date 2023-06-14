using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ItemDropSystem : MonoBehaviour
{
    [Header("EXP")]
    [SerializeField] private GameObject expPickUpPrefab;
    [SerializeField] private GameObjectCollection expPickUpPool; // Pool containing expPicker prefabs    

    [Header("Healing")]
    [SerializeField] private FloatVariable healthDropChance;
    [SerializeField] private GameObject healPickUpPrefab;
    [SerializeField] private GameObjectCollection healPickUpPool;

    
    private void Awake()
    {
        // Setup ScriptableObject Variables
        expPickUpPool.Clear();
        healPickUpPool.Clear();
        healthDropChance.Value = 1 / 2f;

        // Other initialization
        SpawnExpPool();
        SpawnHealingPool();
    }

    // This creates a ExpPicker Pool at the start
    private void SpawnExpPool()
    {
        GameObject expPickUpHolder = new GameObject("Exp Holder");
        for (int i = 0; i < 50; i++)
        {
            GameObject expPickUp = Instantiate(expPickUpPrefab, expPickUpHolder.transform);
            expPickUpPool.Add(expPickUp);
            expPickUp.SetActive(false);
        }
    }


    private void SpawnHealingPool()
    {
        GameObject healthPickUpHolder = new GameObject("Health Holder");
        for (int i = 0; i < 50; i++)
        {
            GameObject healPickUp = Instantiate(healPickUpPrefab, healthPickUpHolder.transform);
            healPickUpPool.Add(healPickUp);
            healPickUp.SetActive(false);
        }
    }
}
