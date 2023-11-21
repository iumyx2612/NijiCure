using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;


[RequireComponent(typeof(CircleCollider2D))]
public class HealPickUp : MonoBehaviour, IPickUpItem
{
    private CircleCollider2D selfCollider;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 basePosition;
    private int healAmount;
    private Vector2 baseScale;

    [SerializeField] private IntGameEvent healPlayer;
    [SerializeField] private IntVariable playerBaseHealth;
    [SerializeField] private FloatVariable healMultiplier; // Setup in ItemDropSystem.cs
    
    void Awake()
    {
        selfCollider = gameObject.GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        healAmount = Mathf.RoundToInt(playerBaseHealth.Value / 5 * healMultiplier.Value); // Will always heal 20% of player's maxHP (HoloCure)
        transform.localScale = baseScale;
        basePosition = transform.position;

        StartCoroutine(Animate());
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
        healPlayer.Raise(healAmount); // Check PlayerCombat.cs
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
