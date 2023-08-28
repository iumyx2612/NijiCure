using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Counter/Move Speed")]
public class MoveSpeedCounterData : CounterBaseData
{
    public bool singleAbility; // If the Counter can stack with others: false
    public bool increase; // If the Counter increase or decrease speed
    [HideInInspector] public float percentage;
    [HideInInspector] public string abilityName;
    // To play the animation when we place counter onto the Enemy
    [HideInInspector] public List<GameObject> counterPool;
    [HideInInspector] public GameObject counterPrefab;
}


public class MoveSpeedCounter : Counter<MoveSpeedCounterData>
{
    // Base
    public string counterName;
    public int maxNum;
    public float existTime;
    public float internalTime = 0;
    public int currentNum = 0;

    // Move speed
    public bool singleAbility;
    public bool increase;
    public float percentage;
    public string abilityName;
    private List<GameObject> counterPool;
    private GameObject counterPrefab;

    public void SetData(MoveSpeedCounterData data)
    {
        counterName = data.counterName;
        maxNum = data.maxNum;
        existTime = data.existTime;

        singleAbility = data.singleAbility;
        increase = data.increase;
        percentage = data.percentage;
        abilityName = data.abilityName;
        counterPool = data.counterPool;
        counterPrefab = data.counterPrefab;
    }

    public void Active(Vector2 position)
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
            GameObject counter = GameObject.Instantiate(counterPrefab, counterHolder.transform);
            counter.transform.position = position;
            counterPool.Add(counter);
        }
    }
}
