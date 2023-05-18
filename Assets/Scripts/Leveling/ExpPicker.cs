using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

// This script is attached on the exp particle
public class ExpPicker : MonoBehaviour
{
    [SerializeField] private ExpPickerData expdata;
    
    // Data
    private bool hasLoadedData = false;
    public int type;
    private int exp;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private IntGameEvent increaseExp;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (expdata != null && !hasLoadedData)
        {
            LoadData(expdata);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            increaseExp.Raise(exp);
            gameObject.SetActive(false);
        }
    }

    public void LoadData(ExpPickerData data)
    {
        expdata = data;
        type = data.type;
        exp = data.exp;
        spriteRenderer.sprite = data.sprite;
        hasLoadedData = true;
    }
}
