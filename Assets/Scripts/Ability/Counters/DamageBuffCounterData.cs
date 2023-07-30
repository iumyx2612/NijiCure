using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Counter/Damage Buff")]
public class DamageBuffCounterData : CounterBaseData
{
    public bool singleAbility;
    // Setup in Ability
    [HideInInspector, Range(0f, 1f)] public float damageBuff;
    [HideInInspector] public string abilityName;
    // To play the animation when we place counter onto the Enemy
    [HideInInspector] public List<GameObject> counterPool;
    [HideInInspector] public GameObject counterPrefab;
}


public class DamageBuffCounter : Counter<DamageBuffCounterData>
{
    // Base
    public string counterName;
    public int maxNum;
    public float existTime;
    public float internalTime = 0;
    public int currentNum = 0;
    
    // Damge Buff
    public bool singleAbility;
    public float damageBuff;
    public string abilityName;
    // To play the animation
    private GameObject counterPrefab;
    private List<GameObject> counterPool;

    public void SetData(DamageBuffCounterData data)
    {
        counterName = data.counterName;
        maxNum = data.maxNum;
        existTime = data.existTime;

        singleAbility = data.singleAbility;
        damageBuff = data.damageBuff;
        abilityName = data.abilityName;
        counterPrefab = data.counterPrefab;
        counterPool = data.counterPool;
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
