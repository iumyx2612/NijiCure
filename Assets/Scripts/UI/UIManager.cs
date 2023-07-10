using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("EXP")]
    // Exp stuff
    [SerializeField] private IntVariable currentLevel; // Setup in LevelingSystem.cs
    [SerializeField] private IntVariable currentExp; // Setup in LevelingSystem.cs
    [SerializeField] private IntVariable expToNextLevel; // Setup in LevelingSystem.cs
    [SerializeField] private GameEvent increaseLevelUI; 
    [SerializeField] private GameEvent increaseExpUI; 
    public Image expBarImage;
    public TMP_Text levelDisplayText;

    [Header("Time")]
    [SerializeField] private TMP_Text timeDisplayText;
    [SerializeField] private FloatVariable timeSinceGameStart; // Setup in GameManager.cs

    [Header("Abilities When Level Up")] 
    [SerializeField] private GameObject lvlUpPanel;
    [SerializeField] private List<Button> abilityPickerButtons;
    [SerializeField] private List<Image> abilityPickerImages;
    [SerializeField] private List<TMP_Text> abilityPickerNames;
    [SerializeField] private List<TMP_Text> abilityPickerDescs;
    [SerializeField] private GameEvent levelUpAbilityUIPopUp; // Raised in AbilityManager.cs
    [SerializeField] private AbilityGameEvent modifyAbility; // Setup in AbilityManager.cs
    [SerializeField] private AbilityCollection abilitiesToPick; // Setup in AbilityManager.cs
    [SerializeField] private AbilityCollection currentAbilities; // Setup in AbilityManager.cs 

    [SerializeField] private List<Image> countdownImages;
    [SerializeField] private PassiveAbilityGameEvent activeCountdownImage; // Raised by some Passive Abilities

    [Header("Coin and Kill Info")] 
    [SerializeField] private GameEvent updateCoinInfo; // Raise in CoinPickUp.cs
    [SerializeField] private IntVariable stageCoinAmount; // Setup in CoinPickUp.cs
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private GameEvent updateKillInfo; // Raise in EnemyMovement.cs
    [SerializeField] private IntVariable stageKillAmount; // Setup in EnemyMovement.cs
    [SerializeField] private TMP_Text killAmountText;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        stageKillAmount.Value = 0;
//        stageCoinAmount.Value = 0;
        increaseExpUI.AddListener(ChangeExpBar);
        increaseLevelUI.AddListener(ChangeLevelDisplay);
        levelUpAbilityUIPopUp.AddListener(LevelUpAbilityUIPopUp);
        activeCountdownImage.AddListener(ActiveCountdownImage);
//        updateCoinInfo.AddListener(UpdateCoinInfo);
        updateKillInfo.AddListener(UpdateKillInfo);
    }

    private void OnDisable()
    {
        increaseExpUI.RemoveListener(ChangeExpBar);
        increaseLevelUI.RemoveListener(ChangeLevelDisplay);
        levelUpAbilityUIPopUp.RemoveListener(LevelUpAbilityUIPopUp);
        activeCountdownImage.RemoveListener(ActiveCountdownImage);
//        updateCoinInfo.RemoveListener(UpdateCoinInfo);
        updateKillInfo.RemoveListener(UpdateKillInfo);
    }

    // Update is called once per frame
    void Update()
    {
        TimeFloatToString();
    }
    
    // --------------- EXP section ---------------
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
    
    private void LevelUpAbilityUIPopUp()
    {
        if (!lvlUpPanel.activeSelf)
        {
            // Pause the Game
            Time.timeScale = 0f;
            lvlUpPanel.SetActive(true);
            for (int i = 0; i < abilitiesToPick.Count; i++)
            {
                AbilityBase ability = abilitiesToPick[i];
                // Setup the UI
                Image abilityPickerImg = abilityPickerImages[i];
                TMP_Text abilityPickerName = abilityPickerNames[i];
                TMP_Text abilityPickerDesc = abilityPickerDescs[i];

                abilityPickerImg.sprite = ability.UISprite;
                abilityPickerName.text = ability.abilityName;
                abilityPickerDesc.text = ability.description;

                // Check if player already had the Ability
                if (currentAbilities.Contains(ability))
                {
                    // If yes, display the upgrade info
                    AbilityBase _ability = ability.GetUpgradeDataInfo();
                    abilityPickerImg.sprite = _ability.UISprite;
                    abilityPickerName.text = _ability.abilityName;
                    abilityPickerDesc.text = _ability.description;
                }
                
                // Setup the Button action
                Button abilityPickerBtn = abilityPickerButtons[i];
                abilityPickerBtn.onClick.AddListener(()=>modifyAbility.Raise(ability));
            }
        }
        else
        {
            // Loop through all button to unsubscribe event
            foreach (Button btn in abilityPickerButtons)
            {
                btn.onClick.RemoveAllListeners();
            }
            // Unpause the Game
            Time.timeScale = 1f;
            lvlUpPanel.SetActive(false);
        }
    }
    // --------------- Time ---------------
    private void TimeFloatToString()
    {
        int minutes = (int) timeSinceGameStart.Value / 60;
        int seconds = (int) timeSinceGameStart.Value % 60;
        string timeString = minutes.ToString("00") + " : " + seconds.ToString("00");
        timeDisplayText.text = timeString;
    }

    // --------------- Countdown Image ---------------
    private void ActiveCountdownImage(PassiveAbilityInfo info)
    {
        for (int i = 0; i < countdownImages.Count; i++)
        {
            Image image = countdownImages[i];
            CountdownImage cdImage = image.GetComponent<CountdownImage>();
            // Check if the Ability call to this is already been displayed
            if (info.UISprite == cdImage.GetSprite() && image.IsActive())
            {
                cdImage.ResetInternalDuration();
                break;
            }
            if (!image.IsActive())
            {
                cdImage.SetSprite(info.UISprite);
                cdImage.SetDuration(info.duration);
                image.gameObject.SetActive(true);
                break;
            }
        }
    }
    
    // --------------- Coin and Kill section ---------------
    private void UpdateCoinInfo()
    {
        coinAmountText.text = stageCoinAmount.Value.ToString();
    }

    private void UpdateKillInfo()
    {
        killAmountText.text = stageKillAmount.Value.ToString();
    }
    
}
