using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// The Data will be assigned to a Ability
/// Ability will handle the Instantiating the Item Drop Pool
/// All the parameters of Data will be parse through the Ability
/// Counter will be placed onto the Enemies through the activation of the Ability
/// The Counter which is placed onto the Enemies is ItemDropCounter
/// Its data is parse through the ItemDropCounterData during Counter placement process 
/// </summary>

[CreateAssetMenu(menuName = "Ability/Counter/Item Drop")]
public class ItemDropCounterData : CounterBaseData
{
    // Setup using Ability Script
    [HideInInspector] public string abilityName;
    [HideInInspector] public GameObject itemDropPrefab;
    [HideInInspector] public float dropChance;
    [HideInInspector] public List<GameObject> itemDropPool; // Don't forget to CLEAR the pool
}


/// <summary>
/// This Counter exists inside the Enemies. It will drop an Item when Enemy dies
/// </summary>
public class ItemDropCounter : Counter<ItemDropCounterData>
{
    // Base
    public string counterName;
    public int maxNum;
    public float existTime;
    public float internalTime = 0;
    public int currentNum = 0;

    // Item Drop
    private string abilityName;
    private GameObject itemDropPrefab;
    private float dropChance;
    private List<GameObject> itemDropPool;

    public void SetData(ItemDropCounterData data)
    {
        counterName = data.counterName;
        maxNum = data.maxNum;
        existTime = data.existTime;
        abilityName = data.abilityName;
        itemDropPrefab = data.itemDropPrefab;
        dropChance = data.dropChance;
        itemDropPool = data.itemDropPool;
    }

    // Implement the Drop Item
    public void Active(Vector2 position)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < dropChance)
        {
            bool hasEnoughItem = false;
            for (int i = 0; i < itemDropPool.Count; i++)
            {
                GameObject itemDrop = itemDropPool[i];
                if (!itemDrop.activeSelf)
                {
                    itemDrop.SetActive(true);
                    itemDrop.transform.position = position;
                    hasEnoughItem = true;
                    break;
                }
            }

            // If doesn't have inaff item, then create new Item
            if (!hasEnoughItem)
            {
                GameObject counterHolder = GameObject.Find(abilityName + " Counters"); // To keep things organized
                GameObject itemDrop = GameObject.Instantiate(itemDropPrefab, counterHolder.transform);
                itemDrop.transform.position = position;
                itemDropPool.Add(itemDrop);
            }
        }    
    }
}
