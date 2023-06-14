using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GameManager : MonoBehaviour
{
    [Header("Time related stuff")]
    [SerializeField] private FloatVariable timeSinceGameStart;
    
    [Header("Leveling")]
    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private IntVariable currentExp;
    [SerializeField] private IntVariable expToNextLevel; // Initialize with 79 (Wiki)
    [SerializeField] private IntGameEvent increaseExp;
    [SerializeField] private GameEvent increaseLevel;
    // UI for leveling
    [SerializeField] private GameEvent increaseLevelUI; // Setup in UIManager.cs 
    [SerializeField] private GameEvent increaseExpUI; // Setup in UIManager.cs
    
    [Header("Modify Abilities when LvlUp Event")]
    [SerializeField] private GameEvent levelUpSetUpAbility; // Setup in AbilityManager.cs
    

    private void Awake()
    {
        // Setup ScriptableObject Variables
        timeSinceGameStart.Value = 0f;
        currentLevel.Value = 1;
        currentExp.Value = 0;
        expToNextLevel.Value = 79;
        // Setup ScriptableObject GameEvents
        increaseExp.AddListener(IncreaseExp);
        increaseLevel.AddListener(IncreaseLevel);
    }
    
    // Update is called once per frame
    void Update()
    {
        timeSinceGameStart.Value += Time.deltaTime;
    }
    
    private void OnDisable()
    {
        increaseExp.RemoveListener(IncreaseExp);
        increaseLevel.RemoveListener(IncreaseLevel);
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
        levelUpSetUpAbility.Raise(); // Check AbilityManager.cs
    }
    
    // Will be called every time we level up
    private void CalculateExpForNextLevel()
    {
        int subtrahend = Mathf.RoundToInt(Mathf.Pow(4 * (currentLevel + 1), 2.1f));
        int minuend = Mathf.RoundToInt(Mathf.Pow(4 * currentLevel, 2.1f));
        expToNextLevel.Value = subtrahend - minuend;
    }
}
