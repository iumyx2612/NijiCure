using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class BanMaiPudding : MonoBehaviour, IPickUpItem
{
    private CircleCollider2D selfCollider;
    private Vector2 baseScale;
    // Data
    private int healthIncrease;
    [SerializeField] private IntGameEvent healPlayer;
    [SerializeField] private GameEvent puddingHunterGameEvent; // Setup in PuddingHunter.cs

    private void Awake()
    {
        selfCollider = GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = baseScale;
    }

    public void OnPickUp(Transform player)
    {
        // Move the ExpItem towards the player
        Vector2 dest = player.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(dest, 0.3f));
        sequence.Join(transform.DOScale(new Vector2(0f, 0f), 0.3f));
        sequence.OnComplete(DonePickUp);
    }

    private void DonePickUp()
    {
        puddingHunterGameEvent.Raise(); // Check PuddingHunter.cs
        healPlayer.Raise(healthIncrease); // Check PlayerCombat.cs
        gameObject.SetActive(false);
    }

    public void LoadData(PuddingHunterData _data)
    {
        healthIncrease = _data.healthIncrease;
    }
}
