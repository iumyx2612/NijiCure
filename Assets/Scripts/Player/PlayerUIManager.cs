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
    private GameObject player;
    
    // Health Bar
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
    [SerializeField] private Color lowerHealthColor = new Color(255, 50, 50, 1);
    [SerializeField] private Color increaseHealthColor = new Color(104, 255, 104, 1);


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        // To reduce the number of update called
        // Health Bar Image
        if (healthBarImageFull.gameObject.activeSelf)
        {
            healthBarImageFull.transform.position = new Vector2(player.transform.position.x,
                player.transform.position.y + baseHealthBarYPos); 
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
            if (!healthPopupText.gameObject.activeSelf)
            {
                healthPopupText.text = Mathf.Abs(amount).ToString();
                GameObject healthPopupTextGO = healthPopupText.gameObject;
                healthPopupTextGO.transform.position = player.transform.position;
                if (amount < 0)
                {
                    healthPopupText.color = lowerHealthColor;
                }
                else
                {
                    healthPopupText.color = increaseHealthColor;
                }

                float basePosY = healthPopupTextGO.transform.position.y;
                float basePosX = healthPopupTextGO.transform.position.x;
                healthPopupTextGO.SetActive(true);
                Sequence textSequence = DOTween.Sequence();
                textSequence.Append(healthPopupTextGO.transform.DOMove(new Vector2(basePosX + Random.Range(-0.2f, 0.2f),
                    basePosY + 0.5f), 0.6f));
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
