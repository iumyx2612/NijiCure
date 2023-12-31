using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private List<Image> abilityTypeIcons;
    [SerializeField] private List<TMP_Text> abilityPickerNames;
    [SerializeField] private List<TMP_Text> abilityPickerDescs;
    [SerializeField] private GameEvent levelUpAbilityUIPopUp; // Raised in AbilityManager.cs
    [SerializeField] private AbilityGameEvent modifyAbility; // Setup in AbilityManager.cs
    [SerializeField] private AbilityCollection abilitiesToPick; // Setup in AbilityManager.cs
    [SerializeField] private AbilityCollection currentAbilities; // Setup in AbilityManager.cs 

    [Header("Timer for Passive Abilities")]
    [SerializeField] private List<Image> countdownImages;
    [SerializeField] private PassiveAbilityGameEvent activeCountdownImage; // Raised by some Passive Abilities

    [Header("Coin and Kill Info")] 
    [SerializeField] private GameEvent updateCoinInfo; // Raise in CoinPickUp.cs
    [SerializeField] private IntVariable stageCoinAmount; // Setup in CoinPickUp.cs
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private GameEvent updateKillInfo; // Raise in EnemyMovement.cs
    [SerializeField] private IntVariable stageKillAmount; // Setup in EnemyMovement.cs
    [SerializeField] private TMP_Text killAmountText;

    [Header("Player Panel Info")] 
    [SerializeField] private Sprite firstRowIcon;
    [SerializeField] private Sprite secondRowIcon;
    [SerializeField] private PlayerData stagePlayerData;
    [SerializeField] private Image charIcon;
    [SerializeField] private List<Image> firstRowAbilityIcons;
    [SerializeField] private List<Image> secondRowAbilityIcons;
    [SerializeField] private AbilityGameEvent updateAbilityPanel; // Called in AbilityManager.cs
    
    [Header("Pause Menu")]
    [SerializeField] private BoolVariable gameIsPause;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameEvent playerDie;
    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameEvent endStage;
    [SerializeField] private GameObject winPanel;

    [Header("Abilites Info")]
    [SerializeField] private List<GameObject> abilityInfoImages;
    [SerializeField] private AbilityGameEvent updateAbilityInfo; // Called in AbilityManager.cs

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
        updateAbilityPanel.AddListener(UpdateAbilityPanel);
        updateAbilityInfo.AddListener(UpdateAbilityInfo);
        playerDie.AddListener(DeadPanel);
        endStage.AddListener(WinPanel);
        gameIsPause.Value = false;
        UINavigation.Instance.cancelAction.action.performed += PauseOrUnpause;
        for (int i = 0; i < abilityInfoImages.Count; i++)
        {
            abilityInfoImages[i].SetActive(false);
        }
    }

    private void Start()
    {
        UpdateCharIcon();
    }

    private void OnDisable()
    {
        increaseExpUI.RemoveListener(ChangeExpBar);
        increaseLevelUI.RemoveListener(ChangeLevelDisplay);
        levelUpAbilityUIPopUp.RemoveListener(LevelUpAbilityUIPopUp);
        activeCountdownImage.RemoveListener(ActiveCountdownImage);
//        updateCoinInfo.RemoveListener(UpdateCoinInfo);
        updateKillInfo.RemoveListener(UpdateKillInfo);
        updateAbilityPanel.RemoveListener(UpdateAbilityPanel);
        updateAbilityInfo.RemoveListener(UpdateAbilityInfo);
        playerDie.RemoveListener(DeadPanel);
        endStage.RemoveListener(WinPanel);
        UINavigation.Instance.cancelAction.action.performed -= PauseOrUnpause;
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
            gameIsPause.Value = true;
            lvlUpPanel.SetActive(true);
            for (int i = 0; i < abilitiesToPick.Count; i++)
            {
                AbilityBase ability = abilitiesToPick[i];
                // Setup the UI
                Image abilityPickerImg = abilityPickerImages[i];
                Image abilityTypeIcon = abilityTypeIcons[i];
                TMP_Text abilityPickerName = abilityPickerNames[i];
                TMP_Text abilityPickerDesc = abilityPickerDescs[i];

                abilityPickerImg.sprite = ability.abilityIcon;
                abilityTypeIcon.sprite = ability.typeIcon;
                abilityPickerName.text = ability.abilityName;
                abilityPickerDesc.text = ability.description;

                // Check if player already had the Ability
                if (currentAbilities.Contains(ability))
                {
                    // If yes, display the upgrade info
                    AbilityBase _ability = ability.GetUpgradeDataInfo();
                    abilityPickerImg.sprite = _ability.abilityIcon;
                    abilityTypeIcon.sprite = _ability.typeIcon;
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
            gameIsPause.Value = false;
        }
    }

    private void UpdateAbilityInfo(AbilityBase _ability)
    {
        for (int i = 0; i < abilityInfoImages.Count; i++)
        {
            GameObject abilityInfoImage = abilityInfoImages[i];
            AbilityInfoImage script = abilityInfoImage.GetComponent<AbilityInfoImage>();
            if (script.ability == null)
            {
                script.SetUp(_ability);
                abilityInfoImage.SetActive(true);
                break;
            }
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
                cdImage.UpdateInfo(info);
                break;
            }
            if (!image.IsActive())
            {
                cdImage.UpdateInfo(info);
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
    
    // --------------- Char Info section ---------------
    private void UpdateCharIcon()
    {
        charIcon.sprite = stagePlayerData.playerIcon;
    }

    private void UpdateAbilityPanel(AbilityBase ability)
    {
        if (!(ability is IntStatsBuff) && !(ability is FloatStatsBuff))
        {
            // Not from player
            if (ability.playerType != stagePlayerData.type)
            {
                for (int i = 0; i < secondRowAbilityIcons.Count; i++)
                {
                    Image icon = secondRowAbilityIcons[i];
                    if (icon.sprite == secondRowIcon)
                    {
                        icon.sprite = ability.abilityIcon;
                        Color temp = icon.color;
                        temp.a = 1;
                        icon.color = temp; 
                        break;
                    }
                }
            }
            else if (ability.playerType == stagePlayerData.type)
            {
                for (int i = 0; i < firstRowAbilityIcons.Count; i++)
                {
                    Image icon = firstRowAbilityIcons[i];
                    if (icon.sprite == firstRowIcon)
                    {
                        icon.sprite = ability.abilityIcon;
                        Color temp = icon.color;
                        temp.a = 1;
                        icon.color = temp; 
                        break;
                    }
                }
            }
        }
    }
    
    // --------------- Pause section ---------------
    public void PauseOrUnpause(InputAction.CallbackContext ctx)
    {
        if (!gameIsPause.Value && !pausePanel.activeSelf) // Game is not pause
        {
            gameIsPause.Value = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (gameIsPause.Value && pausePanel.activeSelf)
        {
            gameIsPause.Value = false;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        gameIsPause.Value = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CharSelectionScene()
    {
        Resume();
        SceneManager.LoadScene(1);
    }

    public void MainMenuScene()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DeadPanel()
    {
        gameIsPause.Value = true;
        Time.timeScale = 0f;
        deadPanel.SetActive(true);
    }

    private void WinPanel()
    {
        gameIsPause.Value = true;
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }
}
