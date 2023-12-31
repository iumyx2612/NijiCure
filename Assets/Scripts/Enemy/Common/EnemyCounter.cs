using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private Image spdCounterImage;
    [SerializeField] private Image dmgCounterImage;
    [SerializeField] private Image itemDropCounterImage;
    
    private IBaseEnemyBehavior iBaseEnemyBehaviorScript;
    private EnemyCombat enemyCombatScript;
    private EnemyDrop enemyDropScript;
    
    // Item Drop Counters
    private Dictionary<string, ItemDropCounter> stringToItemDropCounter = new 
        Dictionary<string, ItemDropCounter>();
    
    // Damage buff Counters
    private Dictionary<string, DamageBuffCounter> stringToDmgBuffCounter = new 
        Dictionary<string, DamageBuffCounter>();
    
    // Move speed Counters
    private Dictionary<string, MoveSpeedCounter> stringToMoveSpdCounter = new 
        Dictionary<string, MoveSpeedCounter>();
    
    
    private void Awake()
    {
        iBaseEnemyBehaviorScript = GetComponent<IBaseEnemyBehavior>();
        enemyCombatScript = GetComponent<EnemyCombat>();
        enemyDropScript = GetComponent<EnemyDrop>();
    }

    // Reset all UI elements
    private void OnDisable()
    {
        ClearDmgBuffCounters();
        ClearItemDropCounters();
        ClearMoveSpdCounters();
        dmgCounterImage.gameObject.SetActive(false);
        itemDropCounterImage.gameObject.SetActive(false);
        spdCounterImage.gameObject.SetActive(false);
    }

    // Use LateUpdate here since Counter will be placed by Abilities during Update
    // We want to start timer countdown right after Counters are placed 
    // Consider using Unity Job System for multi thread 
    private void LateUpdate()
    {
        // -------------- Update timer of ItemDropCounter --------------
        for (int i = stringToItemDropCounter.Count - 1; i >= 0; i--)
        {
            string counterName = stringToItemDropCounter.Keys.ElementAt(i);
            // We use reverse order since we want to remove the Counter if time reach
            ItemDropCounter itemDropCounter = stringToItemDropCounter.Values.ElementAt(i);
            itemDropCounter.internalTime += Time.deltaTime;
            if (itemDropCounter.internalTime >= itemDropCounter.existTime)
            {
                itemDropCounter.currentNum = 0;
                itemDropCounter.internalTime = 0f;
                stringToItemDropCounter.Remove(counterName);
                // Update the List in EnemyDrop cuz it lose Counter
                UpdateItemDropCounter();
            }
        }
        UpdateItemDropCounterUI();
        // -------------- Update timer of DamageBuffCounter --------------
        for (int i = stringToDmgBuffCounter.Count - 1; i >= 0; i--)
        {
            string counterName = stringToDmgBuffCounter.Keys.ElementAt(i);
            // We use reverse order since we want to remove the Counter if time reach
            DamageBuffCounter dmgBuffCounter = stringToDmgBuffCounter.Values.ElementAt(i);
            dmgBuffCounter.internalTime += Time.deltaTime;
            if (dmgBuffCounter.internalTime >= dmgBuffCounter.existTime)
            {
                dmgBuffCounter.currentNum = 0;
                dmgBuffCounter.internalTime = 0f;
                stringToDmgBuffCounter.Remove(counterName);
                // Update the List in EnemyCombat cuz it lose Counter
                UpdateDmgBuffCounter();
            }
        }
        UpdateDmgBuffCounterUI();
        // -------------- Update timer of MoveSpeedCounter --------------
        for (int i = stringToMoveSpdCounter.Count - 1; i >= 0; i--)
        {
            string counterName = stringToMoveSpdCounter.Keys.ElementAt(i);
            // We use reverse order since we want to remove the Counter if time reach
            MoveSpeedCounter moveSpdCounter = stringToMoveSpdCounter.Values.ElementAt(i);
            moveSpdCounter.internalTime += Time.deltaTime;
            if (moveSpdCounter.internalTime >= moveSpdCounter.existTime)
            {
                moveSpdCounter.currentNum = 0;
                moveSpdCounter.internalTime = 0f;
                stringToMoveSpdCounter.Remove(counterName);
                // Update the List in EnemyCombat cuz it lose Counter
                UpdateMoveSpdCounter();
            }
        }
        UpdateMoveSpdCounterUI();
    }

    // ------------------ Item Drop Counter ------------------
    public int GetNumItemDropCounter(string counterName)
    {
        if (stringToItemDropCounter.ContainsKey(counterName))
        {
            ItemDropCounter counter = stringToItemDropCounter[counterName];
            return counter.currentNum;   
        }

        return 0;
    }

    public ItemDropCounter GetItemDropCounter(string counterName)
    {
        return stringToItemDropCounter[counterName];
    }
    
    public void AddItemDropCounter(ItemDropCounter _counter)
    {
        // Make a copy of the counter
        // If the added Counter exists
        if (stringToItemDropCounter.ContainsKey(_counter.counterName))
        {
            ItemDropCounter counter = stringToItemDropCounter[_counter.counterName];
            // If the Counter hasn't reach the limit
            if (counter.currentNum < counter.maxNum)
            {
                counter.currentNum += 1;
            }
            // Reset the counter timer
            counter.internalTime = 0f;
        }
        else
        {
            ItemDropCounter addedCounter = new ItemDropCounter(_counter);
            addedCounter.LinkPool(_counter);
            addedCounter.currentNum += 1;
            stringToItemDropCounter.Add(addedCounter.counterName, addedCounter);
            // Update the List in EnemyDrop cuz it has new Counter
            UpdateItemDropCounter();
        }
    }

    public void RemoveItemDropCounter(string counterName)
    {
        if (stringToItemDropCounter.ContainsKey(counterName))
        {
            stringToItemDropCounter.Remove(counterName);
        }
    }

    private void UpdateItemDropCounter()
    {
        List<ItemDropCounter> values = stringToItemDropCounter.Values.ToList();
        enemyDropScript.itemDropCounters = new List<ItemDropCounter>(values);
    }

    private void UpdateItemDropCounterUI()
    {
        // If the Image is not active, we simply active it
        if (!itemDropCounterImage.IsActive() && stringToItemDropCounter.Count > 0)
        {
            itemDropCounterImage.gameObject.SetActive(true);
        }
        // If the Image is already active
        else if (itemDropCounterImage.IsActive())
        {
            // Check if Enemy still have Counters
            if (stringToItemDropCounter.Count <= 0)
            {
                itemDropCounterImage.gameObject.SetActive(false);   
            }
        }
    }

    private void ClearItemDropCounters()
    {
        stringToItemDropCounter.Clear();
    }
    
    // ------------------ Damage Buff Counter ------------------
    public int GetNumDmgBuffCounter(DamageBuffCounter _counter)
    {
        if (stringToDmgBuffCounter.ContainsKey(_counter.counterName))
        {
            DamageBuffCounter counter = stringToDmgBuffCounter[_counter.counterName];
            return counter.currentNum;   
        }

        return 0;
    }
    
    public DamageBuffCounter GetDmgBuffCounter(string counterName)
    {
        return stringToDmgBuffCounter[counterName];
    }
    
    public void AddDmgBuffCounter(DamageBuffCounter _counter)
    {
        // If the added Counter exists
        if (stringToDmgBuffCounter.ContainsKey(_counter.counterName))
        {
            DamageBuffCounter counter = stringToDmgBuffCounter[_counter.counterName];
            // If the Counter hasn't reach the limit
            if (counter.currentNum < counter.maxNum)
            {
                counter.currentNum += 1;
            }
            // Reset the counter timer
            counter.internalTime = 0f;
        }
        else
        {
            DamageBuffCounter addedCounter = new DamageBuffCounter(_counter);
            addedCounter.currentNum += 1;
            stringToDmgBuffCounter.Add(addedCounter.counterName, addedCounter);
            // Update the List in EnemyDrop cuz it has new Counter
            UpdateDmgBuffCounter();
        }
    }

    public void RemoveDmgBuffCounter(string counterName)
    {
        if (stringToDmgBuffCounter.ContainsKey(counterName))
        {
            stringToDmgBuffCounter.Remove(counterName);
        }
    }
    
    private void UpdateDmgBuffCounter()
    {
        List<DamageBuffCounter> values = stringToDmgBuffCounter.Values.ToList();
        enemyCombatScript.dmgBuffCounters = new List<DamageBuffCounter>(values);
    }
    
    private void UpdateDmgBuffCounterUI()
    {
        // If the Image is not active, we simply active it
        if (!dmgCounterImage.IsActive() && stringToDmgBuffCounter.Count > 0)
        {
            dmgCounterImage.gameObject.SetActive(true);
        }
        // If the Image is already active
        else if (dmgCounterImage.IsActive())
        {
            // Check if Enemy still have Counters
            if (stringToDmgBuffCounter.Count <= 0)
            {
                dmgCounterImage.gameObject.SetActive(false);   
            }
        }
    }
    
    private void ClearDmgBuffCounters()
    {
        stringToDmgBuffCounter.Clear();
    }
    
    // ------------------ Move Speed Counter ------------------
    public int GetNumMoveSpdCounter(string counterName)
    {
        if (stringToMoveSpdCounter.ContainsKey(counterName))
        {
            MoveSpeedCounter counter = stringToMoveSpdCounter[counterName];
            return counter.currentNum;   
        }

        return 0;
    }
    
    public MoveSpeedCounter GetMoveSpdCounter(string counterName)
    {
        return stringToMoveSpdCounter[counterName];
    }
    
    public void AddMoveSpdCounter(MoveSpeedCounter _counter)
    {
        // If the added Counter exists
        if (stringToMoveSpdCounter.ContainsKey(_counter.counterName))
        {
            MoveSpeedCounter counter = stringToMoveSpdCounter[_counter.counterName];
            // If the Counter hasn't reach the limit
            if (counter.currentNum < counter.maxNum)
            {
                counter.currentNum += 1;
            }
            // Reset the counter timer
            counter.internalTime = 0f;
        }
        else
        {
            // Make a copy of the counter
            MoveSpeedCounter addedCounter = new MoveSpeedCounter(_counter);
            addedCounter.currentNum += 1;
            stringToMoveSpdCounter.Add(addedCounter.counterName, addedCounter);
            // Update the List in EnemyDrop cuz it has new Counter
            UpdateMoveSpdCounter();
        }
    }

    public void RemoveMoveSpdCounter(string counterName)
    {
        if (stringToMoveSpdCounter.ContainsKey(counterName))
        {
            stringToMoveSpdCounter.Remove(counterName);
        }
    }
    
    private void UpdateMoveSpdCounter()
    {
        List<MoveSpeedCounter> values = stringToMoveSpdCounter.Values.ToList();
        iBaseEnemyBehaviorScript.ModifySpdCounter(values);
    }
    
    private void UpdateMoveSpdCounterUI()
    {
        // If the Image is not active, we simply active it
        if (!spdCounterImage.IsActive() && stringToMoveSpdCounter.Count > 0)
        {
            spdCounterImage.gameObject.SetActive(true);
        }
        // If the Image is already active
        else if (spdCounterImage.IsActive())
        {
            // Check if Enemy still have Counters
            if (stringToMoveSpdCounter.Count <= 0)
            {
                spdCounterImage.gameObject.SetActive(false);   
            }
        }
    }
    
    private void ClearMoveSpdCounters()
    {
        stringToMoveSpdCounter.Clear();
    }
}
