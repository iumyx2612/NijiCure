using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Counter/Item Drop")]
public class ItemDropCounter : CounterBase
{
    // Setup using Ability Script
    [HideInInspector] public string abilityName;
    [HideInInspector] public GameObject itemDropPrefab;
    [HideInInspector] public float dropChance;
    [HideInInspector] public List<GameObject> itemDropPool;
    
    // Drop Item at position
    public void Drop(Vector2 position)
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
                GameObject itemDrop = Instantiate(itemDropPrefab, counterHolder.transform);
                itemDrop.transform.position = position;
                itemDropPool.Add(itemDrop);
            }
        }
    }
}
