using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

// This script is attached on the exp particle
[RequireComponent(typeof(CircleCollider2D))]
public class ExpDrop : MonoBehaviour
{
    public ExpData expData;
    
    // Data
    [SerializeField] private FloatVariable pickupRange;
    private int expAmount;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D selfCollider;

    [SerializeField] private IntGameEvent increaseExp;
    [SerializeField] private List<int> expRange; // For merging

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        selfCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        selfCollider.radius = pickupRange.Value;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            increaseExp.Raise(expAmount);
            gameObject.SetActive(false);
        }
    }

    public void LoadData(ExpData data)
    {
        expData = data;
        expAmount = data.expAmount;
    }
}
