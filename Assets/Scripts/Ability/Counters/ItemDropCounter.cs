using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


/// <summary>
/// This Counter exists inside the Enemies. It will drop an Item when Enemy dies
/// </summary>
/// 
[System.Serializable]
public class ItemDropCounter : CounterBase
{
    [Header("Item Drop")]
    [Range(0f, 1f)] public float dropChance;

    [HideInInspector] public float internalTime = 0;
    [HideInInspector] public int currentNum = 0;


    public ItemDropCounter(ItemDropCounter _counter)
    {
        abilityName = _counter.abilityName;
        counterName = _counter.counterName;
        maxNum = _counter.maxNum;
        existTime = _counter.existTime;
        dropChance = _counter.dropChance;
    }

    public void LinkPool(ItemDropCounter _counter)
    {
        counterPrefab = _counter.counterPrefab;
        counterHolder = _counter.counterHolder;
        counterPool = _counter.counterPool;
    }

    // Implement the Drop Item
    public override void Active(Vector2 position)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < dropChance)
        {
            bool hasEnoughItem = false;
            for (int i = 0; i < counterPool.Count; i++)
            {
                GameObject itemDrop = counterPool[i];
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
                GameObject itemDrop = GameObject.Instantiate(counterPrefab, counterHolder.transform);
                itemDrop.transform.position = position;
                counterPool.Add(itemDrop);
            }
        }    
    }
}
