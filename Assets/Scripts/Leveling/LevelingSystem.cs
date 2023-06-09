using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelingSystem : MonoBehaviour
{
    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private IntVariable currentExp;
    [SerializeField] private IntVariable expToNextLevel; // Initialize with 79 (Wiki)
    [SerializeField] private GameObject expPickUpPrefab;
    [SerializeField] private IntGameEvent increaseExp;
    [SerializeField] private GameEvent increaseLevel;

    [SerializeField] private ExpDropGameEvent dropExp;
    [SerializeField] private GameEvent levelUpShowAbility;
    
    // For UI
    [SerializeField] private GameEvent increaseLevelUI; // Setup in UIManager.cs 
    [SerializeField] private GameEvent increaseExpUI; // Setup in UIManager.cs

    // Pool containing expPicker prefabs    
    public List<GameObject> expPickUpPool; 

    private void Awake()
    {
        // Setup ScriptableObject Variables
        currentLevel.Value = 1;
        currentExp.Value = 0;
        expToNextLevel.Value = 79;
        // Setup ScriptableObject GameEvents
        increaseExp.AddListener(IncreaseExp);
        increaseLevel.AddListener(IncreaseLevel);
        dropExp.AddListener(DropExp);
        // Other initialization
        SpawnExpPool();
    }


    private void OnDisable()
    {
        increaseExp.RemoveListener(IncreaseExp);
        increaseLevel.RemoveListener(IncreaseLevel);
        dropExp.RemoveListener(DropExp);
    }

    private void IncreaseExp(int exp)
    {
        currentExp.Value += exp;
        increaseExpUI.Raise();
        if (currentExp >= expToNextLevel)
        {
            increaseLevel.Raise();
        }
    }

    private void IncreaseLevel()
    {
        currentLevel.Value += 1;
        currentExp.Value = 0;
        increaseExpUI.Raise(); // To reset the ExpBar to 0
        increaseLevelUI.Raise();
        CalculateExpForNextLevel();
        levelUpShowAbility.Raise(); // Check AbilityManager.cs
    }

    private void DropExp(ExpData expData)
    {
        bool hasInactiveExpDrop = false;
        for (int i = 0; i < expPickUpPool.Count; i++)
        {
            GameObject expDrop = expPickUpPool[i];
            if (!expDrop.activeSelf)
            {
                expDrop.GetComponent<ExpPickUp>().LoadData(expData);
                expDrop.transform.position = expData.position;
                expDrop.SetActive(true);
                hasInactiveExpDrop = true;
                break;
            }
        }
        
        // If we're out of inactive ExpDrop, create new one
        if (!hasInactiveExpDrop)
        {
            GameObject expPickUp = Instantiate(expPickUpPrefab);
            expPickUp.transform.position = expData.position;
            expPickUpPool.Add(expPickUp);
            expPickUp.GetComponent<ExpPickUp>().LoadData(expData);
        }
    }

    // This creates a ExpPicker Pool at the start
    private void SpawnExpPool()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject expPickUp = Instantiate(expPickUpPrefab, transform);
            expPickUpPool.Add(expPickUp);
            expPickUp.SetActive(false);
        }
    }

    // Will be called every time we level up
    private void CalculateExpForNextLevel()
    {
        int subtrahend = Mathf.RoundToInt(Mathf.Pow(4 * (currentLevel + 1), 2.1f));
        int minuend = Mathf.RoundToInt(Mathf.Pow(4 * currentLevel, 2.1f));
        expToNextLevel.Value = subtrahend - minuend;
    }
}
