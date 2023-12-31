using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounter : MonoBehaviour
{
    private PlayerMovement movementScript;
    private PlayerCombat combatScript;

    // Damage buff Counters
    private Dictionary<string, DamageBuffCounter> stringToDmgBuffCounter = new 
        Dictionary<string, DamageBuffCounter>();
    
    // Move speed Counters
    private Dictionary<string, MoveSpeedCounter> stringToMoveSpdCounter = new 
        Dictionary<string, MoveSpeedCounter>();

    
    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        combatScript = GetComponent<PlayerCombat>();
    }

    // Reset all UI elems
    private void Start()
    {
        ClearDmgBuffCounters();
        ClearMoveSpdCounters();
    }

    // Use LateUpdate here since Counter will be placed by Abilities during Update
    // We want to start timer countdown right after Counters are placed 
    // Consider using Unity Job System for multi thread 
    private void LateUpdate()
    {
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
    }

    // ------------------ Damage Buff Counter ------------------
    public int GetNumDmgBuffCounter(string counterName)
    {
        if (stringToDmgBuffCounter.ContainsKey(counterName))
        {
            DamageBuffCounter counter = stringToDmgBuffCounter[counterName];
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
            // Make a copy of the counter
            DamageBuffCounter addedCounter = new DamageBuffCounter(_counter);
            addedCounter.currentNum += 1;
            stringToDmgBuffCounter.Add(addedCounter.counterName, addedCounter);
        }
        
        UpdateDmgBuffCounter();
    }

    public void RemoveDmgBuffCounter(string counterName)
    {
        if (stringToDmgBuffCounter.ContainsKey(counterName))
        {
            stringToDmgBuffCounter.Remove(counterName);
            UpdateDmgBuffCounter();
        }
    }
    
    private void UpdateDmgBuffCounter()
    {
        List<DamageBuffCounter> values = stringToDmgBuffCounter.Values.ToList();
        combatScript.dmgBuffCounters = new List<DamageBuffCounter>(values);
    }
    private void ClearDmgBuffCounters()
    {
        stringToDmgBuffCounter.Clear();
        UpdateDmgBuffCounter();
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
            addedCounter.internalTime = 0f;
            stringToMoveSpdCounter.Add(addedCounter.counterName, addedCounter);
        }
        UpdateMoveSpdCounter();
    }

    public void RemoveMoveSpdCounter(string counterName)
    {
        if (stringToMoveSpdCounter.ContainsKey(counterName))
        {
            MoveSpeedCounter counter = stringToMoveSpdCounter[counterName];
            stringToMoveSpdCounter.Remove(counterName);
            UpdateMoveSpdCounter();
            // Return the speed for the players
            movementScript.ModifySpeed(counter.totalPercentage, true);
        }
    }
    
    // This function is called whenever the Move speed counter List changes
    private void UpdateMoveSpdCounter()
    {
        List<MoveSpeedCounter> values = stringToMoveSpdCounter.Values.ToList();
        // Update the total percentage of each counter
        for (int i = 0; i < values.Count; i++)
        {
            MoveSpeedCounter counter = values[i];
            counter.totalPercentage = counter.percentage * counter.currentNum;
        }
        movementScript.speedCounters = values;
        movementScript.CounterModifySpeed();
    }
    
    private void ClearMoveSpdCounters()
    {
        stringToMoveSpdCounter.Clear();
        UpdateMoveSpdCounter();
    }
}
