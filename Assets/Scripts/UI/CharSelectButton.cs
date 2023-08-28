using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CharSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button _this;
    [SerializeField] private Image playerIcon;
    [SerializeField] private PlayerData stagePlayerData;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private GameObject charImage;
    
    // Data Display Component
    [SerializeField] private TMP_Text textCharName;
    [SerializeField] private TMP_Text textHPAmount;
    [SerializeField] private TMP_Text textSpdAmount;
    [SerializeField] private TMP_Text textCritAmount;
    
    // Ability Display Component
    [SerializeField] private Image abilityImage;
    [SerializeField] private TMP_Text abilityName;
    [SerializeField] private TMP_Text abilityDesc;
    [SerializeField] private TMP_Text debuffDesc;
    
    // Ultimate Display Component
    [SerializeField] private Image ultimateImage;
    [SerializeField] private TMP_Text ultimateName;
    [SerializeField] private TMP_Text ultimateDesc;

    private void Awake()
    {
        _this = GetComponent<Button>();
        _this.onClick.AddListener(OnPlayerPicked);
        playerIcon.sprite = playerData.playerIcon;
        AudioManager.Instance.AddSound(playerData.playerSound);
    }

    private void OnDisable()
    {
        _this.onClick.RemoveAllListeners();
    }

    // Update the Data Panel on select
    public void OnSelect(BaseEventData eventData)
    {
        charImage.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        // Update Data
        textCharName.text = playerData.characterName;
        textHPAmount.text = playerData.health.ToString();
        textSpdAmount.text = playerData.speed.ToString();
        textCritAmount.text = playerData.critChance * 100 + "%";
        // Update Ability
        abilityImage.sprite = playerData.startingAbility.abilityIcon;
        abilityName.text = playerData.startingAbility.abilityName;
        abilityDesc.text = playerData.startingAbility.description;
        debuffDesc.text = playerData.startingAbility.debuffDesc;
        // Update Ultimate
//        ultimateImage.sprite = playerData.ultimateAbility.
        ultimateName.text = playerData.ultimateAbility.ultimateName;
        ultimateDesc.text = playerData.ultimateAbility.ultimateDescription;

        AudioManager.Instance.Play(playerData.playerSound.audioName);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        AudioManager.Instance.Stop(playerData.playerSound.audioName);
    }

    private void OnPlayerPicked()
    {
        stagePlayerData.Set(playerData);   
        SceneManager.LoadScene("SampleScene");
    }
}
