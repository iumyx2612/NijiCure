using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;


public class PlayerUIManager : MonoBehaviour
{
    // Health Bar
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private IntVariable playerBaseHealth;
    [SerializeField] private IntVariable playerCurrentHealth;
    [SerializeField] private Vector2GameEvent healthBarImageUpdate;
    [SerializeField] private Image healthBarImageFilled;
    [SerializeField] private Image healthBarImageFull;
    private float baseHealthBarYPos;
    
    private float internalDisplayTime;
    private float displayTime = 3f;
    
    // Health Popup
    [SerializeField] private TMP_Text[] healthPopupTextArr;
    [SerializeField] private IntGameEvent healthTextPopupGameEvent;
    [SerializeField] private Color lowerHealthColor;
    [SerializeField] private Color increaseHealthColor;


    private void Awake()
    {
    }

    private void Start()
    {
        baseHealthBarYPos = healthBarImageFull.transform.position.y;
    }

    private void OnEnable()
    {
        healthBarImageUpdate.AddListener(HealthBarImagePopUp);
        healthTextPopupGameEvent.AddListener(HealthTextPopUp);
    }

    private void OnDisable()
    {
        healthBarImageUpdate.RemoveListener(HealthBarImagePopUp);
        healthTextPopupGameEvent.RemoveListener(HealthTextPopUp);
    }

    private void Update()
    { 
        if (healthBarImageFull.IsActive())
        {
            internalDisplayTime += Time.deltaTime;
            if (internalDisplayTime >= displayTime)
            {
                healthBarImageFull.gameObject.SetActive(false);
                internalDisplayTime = 0f;
            }
        }
    }

    private void HealthBarImagePopUp(Vector2 position)
    {
        healthBarImageFull.transform.position = position;
        healthBarImageFull.gameObject.SetActive(true);
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        internalDisplayTime = 0f;
        float percentage = 1 - (float) playerCurrentHealth.Value / playerBaseHealth.Value;
        healthBarImageFilled.fillAmount = percentage;
    }

    private void HealthTextPopUp(int amount)
    {
        for (int i = 0; i < healthPopupTextArr.Length; i++)
        {
            TMP_Text healthPopupText = healthPopupTextArr[i];
            if (!healthPopupText.IsActive())
            {
                healthPopupText.text = Mathf.Abs(amount).ToString();
                healthPopupText.transform.position = playerPosRef.Value;
                if (amount < 0)
                {
                    healthPopupText.color = lowerHealthColor;
                }
                else
                {
                    healthPopupText.color = increaseHealthColor;
                }

                float basePosY = playerPosRef.Value.y;
                float basePosX = playerPosRef.Value.x;
                healthPopupText.gameObject.SetActive(true);
                Sequence textSequence = DOTween.Sequence();
                textSequence.Append(healthPopupText.transform.DOMove(new Vector2(basePosX + Random.Range(-0.2f, 0.2f),
                    basePosY + 0.8f), 0.6f));
                textSequence.OnComplete(()=>RecoverHealthPopUpText(healthPopupText));
                break;
            }
        }

    }

    private void RecoverHealthPopUpText(TMP_Text healthPopupText)
    {
        healthPopupText.gameObject.SetActive(false);
        Color temp = healthPopupText.color;
        temp.a = 1;
        healthPopupText.color = temp;
    }
}
