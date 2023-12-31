using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DamageBuffCounter : CounterBase
{
    [Header("Damage Buff")]
    
    // Damage Buff
    public bool singleAbility;
    [Range(0f, 1f)] public float damageBuff;    
    // Timer
    [HideInInspector] public float internalTime = 0;
    [HideInInspector] public int currentNum = 0;


    public DamageBuffCounter(DamageBuffCounter _counter)
    {
        abilityName = _counter.abilityName;
        counterName = _counter.counterName;
        maxNum = _counter.maxNum;
        existTime = _counter.existTime;
        singleAbility = _counter.singleAbility;
        damageBuff = _counter.damageBuff;
    }

    public override void Active(Vector2 position)
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
            GameObject counter = GameObject.Instantiate(counterPrefab, counterHolder.transform);
            counter.transform.position = position;
            counterPool.Add(counter);
        }
    }
}
