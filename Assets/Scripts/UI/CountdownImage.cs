using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class CountdownImage : MonoBehaviour
{
    private Image self;
    [SerializeField] private Image childImage;
    [SerializeField] private TMP_Text stackText;
    private float duration;
    private float internalDuration;
    private float percentage;
    private bool isStatic = false;
    
    private void Awake()
    {
        self = GetComponent<Image>();
        ResetInfo();
    }

    private void Update()
    {
        if (!isStatic)
        {
            internalDuration += Time.deltaTime;
            percentage = 1 - internalDuration / duration;
            self.fillAmount = percentage;
            childImage.fillAmount = percentage;
            if (internalDuration >= duration)
            {
                ResetInfo();
            }
        }
    }

    public void UpdateInfo(PassiveAbilityInfo info)
    {
        if (!info.reset)
        {
            duration = info.duration;
            if (childImage.sprite == null)
                childImage.sprite = info.UISprite;
            isStatic = info.isStatic;
            if (info.stacks != 0)
                stackText.text = info.stacks.ToString();
            internalDuration = 0f;
        }
        else
        {
            ResetInfo();
        }
    }

    public void ResetInfo()
    {
        duration = 0f;
        childImage.sprite = null;
        stackText.text = "";
        self.gameObject.SetActive(false);
    }

    public Sprite GetSprite()
    {
        return childImage.sprite;
    }
}
