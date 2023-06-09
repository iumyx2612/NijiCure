using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Exp stuff
    [SerializeField] private IntVariable currentLevel; // Setup in LevelingSystem.cs
    [SerializeField] private IntVariable currentExp; // Setup in LevelingSystem.cs
    [SerializeField] private IntVariable expToNextLevel; // Setup in LevelingSystem.cs
    [SerializeField] private GameEvent increaseLevelUI; 
    [SerializeField] private GameEvent increaseExpUI; 
    public Image expBarImage;
    public TMP_Text levelDisplayText;
    
    // Start is called before the first frame update
    void Start()
    {
        increaseExpUI.AddListener(ChangeExpBar);
        increaseLevelUI.AddListener(ChangeLevelDisplay);
    }

    private void OnDisable()
    {
        increaseExpUI.RemoveListener(ChangeExpBar);
        increaseLevelUI.RemoveListener(ChangeLevelDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeExpBar()
    {
        float percentage = (float)currentExp.Value / expToNextLevel.Value;
        expBarImage.fillAmount = percentage;
    }

    private void ChangeLevelDisplay()
    {
        string levelString = "Lv: " + currentLevel.Value;
        levelDisplayText.text = levelString;
    }
}
