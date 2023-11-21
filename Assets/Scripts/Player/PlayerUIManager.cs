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
    [Header("Health Bar")]    
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private IntVariable playerBaseHealth;
    [SerializeField] private IntVariable playerCurrentHealth;
    [SerializeField] private GameEvent healthBarImageUpdate;
    [SerializeField] private Image healthBarImageFilled;
    [SerializeField] private Image healthBarImageFull;
    
    private float internalDisplayTime;
    private float displayTime = 3f;
    
    [Header("Health Text")]
    [SerializeField] private TMP_Text[] healthPopupTextArr;
    [SerializeField] private IntGameEvent healthTextPopupGameEvent;
    [SerializeField] private Color lowerHealthColor;
    [SerializeField] private Color increaseHealthColor;

    [Header("Status Text")]
    [SerializeField] private TextNColorGameEvent statusTextPopUpGameEvent;
    [SerializeField] private TMP_Text[] statusTextArr;


    private void Awake()
    {
    }

    private void OnEnable()
    {
        healthBarImageUpdate.AddListener(HealthBarImagePopUp);
        healthTextPopupGameEvent.AddListener(HealthTextPopUp);
        statusTextPopUpGameEvent.AddListener(StatusPopUpText);
    }

    private void OnDisable()
    {
        healthBarImageUpdate.RemoveListener(HealthBarImagePopUp);
        healthTextPopupGameEvent.RemoveListener(HealthTextPopUp);
        statusTextPopUpGameEvent.RemoveListener(StatusPopUpText);
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

    private void HealthBarImagePopUp()
    {
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

    private void StatusPopUpText(TextNColor config)
    {
        for (int i = 0; i < statusTextArr.Length; i++)
        {
            TMP_Text statusText = statusTextArr[i];
            if (!statusText.IsActive())
            {
                statusText.text = config.text;
                statusText.color = config.color;

                float basePosY = playerPosRef.Value.y;
                float basePosX = playerPosRef.Value.x;
                statusText.gameObject.SetActive(true);
                Sequence textSequence = DOTween.Sequence();
                textSequence.Append(statusText.transform.DOMove(new Vector2(basePosX + Random.Range(-0.2f, 0.2f),
                    basePosY + 0.8f), 0.6f));
                textSequence.OnComplete(()=>RecoverHealthPopUpText(statusText));
                break;
            }
        }
    }
}
