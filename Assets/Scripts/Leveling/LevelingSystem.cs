using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelingSystem : MonoBehaviour
{
    public int currentLevel;
    public int currentExp;
    private int expToNextLevel = 79; // Initialize with 79 (Wiki)
    [SerializeField] private GameObject expDropPrefab;
    [SerializeField] private IntGameEvent increaseExp;
    [SerializeField] private GameEvent increaseLevel;

    [SerializeField] private ExpDropGameEvent dropExp;
    [SerializeField] private GameEvent levelUpShowAbility;

    // Pool containing expPicker prefabs    
    public List<GameObject> expDropPool; 

    private void Awake()
    {
        increaseExp.AddListener(IncreaseExp);
        increaseLevel.AddListener(IncreaseLevel);
        dropExp.AddListener(DropExp);
        SpawnExpPool();
    }


    private void IncreaseExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= expToNextLevel)
        {
            increaseLevel.Raise();
            currentExp = 0;
        }
    }

    private void IncreaseLevel()
    {
        currentLevel += 1;
        CalculateExpForNextLevel();
        levelUpShowAbility.Raise(); // Check AbilityManager.cs
    }

    private void DropExp(ExpData expData)
    {
        bool hasInactiveExpDrop = false;
        for (int i = 0; i < expDropPool.Count; i++)
        {
            GameObject expDrop = expDropPool[i];
            if (!expDrop.activeSelf)
            {
                expDrop.GetComponent<ExpDrop>().LoadData(expData);
                expDrop.transform.position = expData.position;
                expDrop.SetActive(true);
                hasInactiveExpDrop = true;
                break;
            }
        }
        
        // If we're out of inactive ExpDrop, create new one
        if (!hasInactiveExpDrop)
        {
            GameObject expDrop = Instantiate(expDropPrefab);
            expDrop.transform.position = expData.position;
            expDropPool.Add(expDrop);
            expDrop.GetComponent<ExpDrop>().LoadData(expData);
        }
    }

    // This creates a ExpPicker Pool at the start
    private void SpawnExpPool()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject expPicker = Instantiate(expDropPrefab, transform);
            expDropPool.Add(expPicker);
            expPicker.SetActive(false);
        }
    }

    // Will be called every time we level up
    private void CalculateExpForNextLevel()
    {
        int subtrahend = Mathf.RoundToInt(Mathf.Pow(4 * (currentLevel + 1), 2.1f));
        int minuend = Mathf.RoundToInt(Mathf.Pow(4 * currentLevel, 2.1f));
        expToNextLevel = subtrahend - minuend;
    }
}
