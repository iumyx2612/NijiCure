using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("Drop Exp")] 
    [SerializeField] private GameObjectCollection expPickUpPool; // Setup in ItemDropSystem.cs
    [SerializeField] private GameObject expPickUpPrefab;
    private int expAmount;

    [Header("Drop Healing")] 
    [SerializeField] private FloatVariable healthDropChance; // Setup in ItemDropSystem.cs 
    [SerializeField] private GameObjectCollection healthPickUpPool; // Setup in ItemDropSystem.cs 
    [SerializeField] private GameObject healthPickUpPrefab;
    
    
    // ------------ Drop General stuff ------------
    public void Drop(int _expAmount)
    {
        DropExp(_expAmount);
        DropHealing();
    }
    
    // ------------ Drop Exp stuff ------------
    public void DropExp(int _expAmount)
    {
        expAmount = _expAmount;
        bool hasInactiveExpDrop = false;
        for (int i = 0; i < expPickUpPool.Count; i++)
        {
            GameObject expDrop = expPickUpPool[i];
            if (!expDrop.activeSelf)
            {
                expDrop.GetComponent<ExpPickUp>().LoadData(_expAmount);
                expDrop.transform.position = transform.position;
                expDrop.SetActive(true);
                hasInactiveExpDrop = true;
                break;
            }
        }
        // If we're out of inactive ExpPickUp, create new one
        if (!hasInactiveExpDrop)
        {
            GameObject expPickUp = Instantiate(expPickUpPrefab);
            expPickUp.AddComponent<ExpPickUp>();
            expPickUp.transform.position = transform.position;
            expPickUpPool.Add(expPickUp);
            expPickUp.GetComponent<ExpPickUp>().LoadData(_expAmount);
        }
    }

    // ------------ Drop Healing stuff -----------------
    private void DropHealing()
    {
        // Sample random number
        float randomNumber = Random.Range(0f, 1f);
        // If satisfy, then drop Heal
        if (randomNumber > healthDropChance.Value)
        {
            bool hasInactiveHealingPickUp = false;
            for (int i = 0; i < healthPickUpPool.Count; i++)
            {
                GameObject healthPickUp = healthPickUpPool[i];
                if (!healthPickUp.activeSelf)
                {
                    healthPickUp.transform.position = transform.position;
                    healthPickUp.SetActive(true);
                    hasInactiveHealingPickUp = true;
                    break;
                }
            }
            
            // If we're out of inactive HealthPickUp
            if (!hasInactiveHealingPickUp)
            {
                GameObject healthPickUp = Instantiate(healthPickUpPrefab);
                healthPickUp.transform.position = transform.position;
                healthPickUp.AddComponent<HealPickUp>();
                healthPickUpPool.Add(healthPickUp);
            }
        }
    }
}
