using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;


[System.Serializable]
public class HealPickUpData
{
    public int healAmount;
    public Vector2 position;

    public HealPickUpData(int _healAmount, Vector2 _position)
    {
        healAmount = _healAmount;
        position = _position;
    }
}


[RequireComponent(typeof(CircleCollider2D))]
public class HealPickUp : MonoBehaviour, IPickUpItem
{
    private CircleCollider2D selfCollider;
    private SpriteRenderer spriteRenderer;
    
    public HealPickUpData healData;

    private Vector2 basePosition;
    private int healAmount;
    private Vector2 baseScale;

    [SerializeField] private IntGameEvent healPlayer;
    
    void Awake()
    {
        selfCollider = gameObject.GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = baseScale;
        basePosition = transform.position;
        if (healData != null)
        {
            LoadData(healData);
        }

        StartCoroutine(Animate());
    }

    public void LoadData(HealPickUpData data)
    {
        healData = data;
        healAmount = data.healAmount;
    }

    public void OnPickUp(Transform player)
    {
        Vector2 dest = player.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(dest, 0.3f));
        sequence.Join(transform.DOScale(new Vector2(0f, 0f), 0.3f));
        sequence.OnComplete(DonePickUp);
    }

    private void DonePickUp()
    {
        gameObject.SetActive(false);
        healPlayer.Raise(healAmount); // Check Player/PlayerCombat.cs
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(basePosition.y + 0.07f, 0.4f));
            sequence.Append(transform.DOMoveY(basePosition.y - 0.07f, 0.4f));
            yield return new WaitForSeconds(0.85f);
        }
    }
}
