using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounter : MonoBehaviour
{
    [SerializeField] private Image spdCounterImage;
    [SerializeField] private Image dmgCounterImage;

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
        dmgCounterImage.gameObject.SetActive(false);
        spdCounterImage.gameObject.SetActive(false);
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
        combatScript.dmgBuffCounters = new List<DamageBuffCounter>(values);
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
    private void ClearMoveSpdCounters()
    {
        stringToMoveSpdCounter.Clear();
    }
}
