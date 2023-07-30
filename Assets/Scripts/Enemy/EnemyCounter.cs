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
    [SerializeField] private TMP_Text counterTimer;
    
    private IBaseEnemyBehavior iBaseEnemyBehaviorScript;
    private EnemyCombat enemyCombatScript;
    private EnemyDrop enemyDropScript;
    
    // Item Drop Counters
    private Dictionary<string, ItemDropCounter> stringToItemDropCounter = new 
        Dictionary<string, ItemDropCounter>();
    
    // Damage buff Counters
    private Dictionary<string, DamageBuffCounter> stringToDmgBuffCounter = new 
        Dictionary<string, DamageBuffCounter>();
    
    
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
        dmgCounterImage.gameObject.SetActive(false);
        itemDropCounterImage.gameObject.SetActive(false);
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
            counterTimer.text = dmgBuffCounter.internalTime.ToString("0.00");
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
    }

    // ------------------ Item Drop Counter ------------------
    public int GetNumItemDropCounter(ItemDropCounterData _counter)
    {
        if (stringToItemDropCounter.ContainsKey(_counter.counterName))
        {
            ItemDropCounter counter = stringToItemDropCounter[_counter.counterName];
            return counter.currentNum;   
        }

        return 0;
    }

    public ItemDropCounter GetItemDropCounter(ItemDropCounterData _counter)
    {
        return stringToItemDropCounter[_counter.counterName];
    }
    
    public void AddItemDropCounter(ItemDropCounterData _counter)
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
            ItemDropCounter addedCounter = new ItemDropCounter();
            addedCounter.SetData(_counter);
            addedCounter.currentNum += 1;
            stringToItemDropCounter.Add(addedCounter.counterName, addedCounter);
            // Update the List in EnemyDrop cuz it has new Counter
            UpdateItemDropCounter();
        }
        UpdateItemDropCounterUI();
    }

    public void RemoveItemDropCounter(ItemDropCounterData _counter)
    {
        if (stringToItemDropCounter.ContainsKey(_counter.counterName))
        {
            stringToItemDropCounter.Remove(_counter.counterName);
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
        else
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
    public int GetNumDmgBuffCounter(DamageBuffCounterData _counter)
    {
        if (stringToDmgBuffCounter.ContainsKey(_counter.counterName))
        {
            DamageBuffCounter counter = stringToDmgBuffCounter[_counter.counterName];
            return counter.currentNum;   
        }

        return 0;
    }
    
    public DamageBuffCounter GetDmgBuffCounter(DamageBuffCounterData _counter)
    {
        return stringToDmgBuffCounter[_counter.counterName];
    }
    
    public void AddDmgBuffCounter(DamageBuffCounterData _counter)
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
            // Make a copy of the counter
            DamageBuffCounter addedCounter = new DamageBuffCounter();
            addedCounter.SetData(_counter);
            addedCounter.currentNum += 1;
            stringToDmgBuffCounter.Add(addedCounter.counterName, addedCounter);
            // Update the List in EnemyDrop cuz it has new Counter
            UpdateDmgBuffCounter();
        }
    }

    public void RemoveDmgBuffCounter(DamageBuffCounterData _counter)
    {
        if (stringToDmgBuffCounter.ContainsKey(_counter.counterName))
        {
            stringToDmgBuffCounter.Remove(_counter.counterName);
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
        else
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
}
