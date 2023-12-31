using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MoveSpeedCounter : CounterBase
{
    [Header("Move Speed")]
    [Range(0f, 1f)] public float percentage;
    
    // To manage the counter timer
    [HideInInspector] public float internalTime = 0;
    [HideInInspector] public int currentNum = 0;
    [HideInInspector] public float totalPercentage;

    public MoveSpeedCounter(MoveSpeedCounter _counter)
    {
        abilityName = _counter.abilityName;
        counterName = _counter.counterName;
        maxNum = _counter.maxNum;
        existTime = _counter.existTime;
        percentage = _counter.percentage;
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
