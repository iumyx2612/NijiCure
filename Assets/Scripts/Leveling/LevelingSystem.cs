using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjectArchitecture;

public class LevelingSystem : MonoBehaviour
{
    public List<GameObject> expPickerPrefabList;
    public int currentLevel;
    public int currentExp;
    [SerializeField] private IntCollection nextLevelExp;
    [SerializeField] private IntGameEvent increaseExp;
    [SerializeField] private GameEvent increaseLevel;

    [SerializeField] private ExpDropGameEvent chooseExpGameEvent;
    [SerializeField] private ExpDropGameEvent dropExp;

    // Pool containing expPicker prefabs    
    public List<GameObject> expPool; 

    private void Awake()
    {
        increaseExp.AddListener(IncreaseExp);
        increaseLevel.AddListener(IncreaseLevel);
        chooseExpGameEvent.AddListener(ChooseExpPicker);
        dropExp.AddListener(DropExp);
        SpawnExpPool();
    }

    private void Start()
    {
    }

    private void IncreaseExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= nextLevelExp[currentLevel])
        {
            increaseLevel.Raise();
        }
    }

    private void IncreaseLevel()
    {
        currentLevel += 1;
//        chooseAbility.Raise();
    }
    
    // This choose what ExpPicker to enable
    private void ChooseExpPicker(ExpDrop expDrop)
    {
        List<float> listChances = expDrop.chances;
        // Random a number
        float number = UnityEngine.Random.Range(0f, 1f);
        // Get the index of listChances, which is the level of ExpPicker
        int index = 0;
        for (int i = 0; i < listChances.Count; i++)
        {
            if (number <= listChances[i])
            {
                index = i;
                break;
            }
        }

        expDrop.level = index;
        dropExp.Raise(expDrop);
    }

    // This drop ExpPicker of :parameter: level
    private void DropExp(ExpDrop expDrop)
    {
        foreach (GameObject expPicker in expPool)
        {
            if (expPicker.GetComponent<ExpPicker>().type == expDrop.level)
            {
                expPicker.transform.position = expDrop.position;
                expPicker.SetActive(true);
                break;
            }
        }
    }

    // This creates a ExpPicker Pool at the start
    // TODO: Fix when have more ExpPicker
    private void SpawnExpPool()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject expPicker = Instantiate(expPickerPrefabList[i], transform);
            expPool.Add(expPicker);
            expPicker.SetActive(false);
        }
    }
}
