using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class CountdownImage : MonoBehaviour
{
    private Image self;
    [SerializeField] private Image childImage;
    private float duration;
    private float internalDuration;
    private float percentage;
    
    private void Awake()
    {
        self = GetComponent<Image>();
    }

    private void Update()
    {
        if (self.IsActive())
        {
            internalDuration += Time.deltaTime;
            percentage = 1 - internalDuration / duration;
            self.fillAmount = percentage;
            childImage.fillAmount = percentage;
            if (internalDuration >= duration)
            {
                internalDuration = 0f;
                childImage.sprite = null;
                self.gameObject.SetActive(false);
            }
        }
    }

    public void SetDuration(float _duration)
    {
        duration = _duration;
    }

    public void SetSprite(Sprite _sprite)
    {
        childImage.sprite = _sprite;
    }

    public Sprite GetSprite()
    {
        return childImage.sprite;
    }

    public void ResetInternalDuration()
    {
        internalDuration = 0f;
    }
}
