using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Counter/Damage Buff")]
public class DamageBuffCounter : CounterBase
{
    public bool singleAbiity;
    // Setup in Ability
    [HideInInspector, Range(0f, 1f)] public float damageBuff;
    [HideInInspector] public string abilityName;
    // To play the animation when we place counter onto the Enemy
    [HideInInspector] public List<GameObject> counterPool;
    [HideInInspector] public GameObject counterPrefab;

    public void CounterActive(Vector2 position)
    {
        bool hasEnoughItem = false;
        for (int i = 0; i < counterPool.Count; i++)
        {
            GameObject counter = counterPool[i];
            if (!counter.activeSelf)
            {
                hasEnoughItem = true;
                counter.SetActive(true);
                counter.transform.position = position;
                break;
            }
        }

        // If doesn't have inaff item, then create new Item
        if (!hasEnoughItem)
        {
            GameObject counterHolder = GameObject.Find(abilityName + " Counters"); // To keep things organized
            GameObject counter = Instantiate(counterPrefab, counterHolder.transform);
            counter.transform.position = position;
            counterPool.Add(counter);
        }
    }
}
