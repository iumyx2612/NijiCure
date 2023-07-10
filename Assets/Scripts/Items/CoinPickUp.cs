using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(CircleCollider2D))]
public class CoinPickUp : MonoBehaviour, IPickUpItem
{
    private CircleCollider2D selfCollider;
    [SerializeField] private IntVariable stageCoinAmount;
    [SerializeField] private GameEvent updateCoinInfo;
    [SerializeField] private FloatVariable abilityCoinMultiplier; 
    [SerializeField] private FloatVariable stageCoinMultiplier; // Load in ItemDropSystem.cs
    
    private Vector2 baseScale;
    
    private void Awake()
    {
        selfCollider = gameObject.GetComponent<CircleCollider2D>();
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
        gameObject.SetActive(false);
        stageCoinAmount.Value += (int)(10 * stageCoinMultiplier.Value * abilityCoinMultiplier.Value);
        updateCoinInfo.Raise(); // Check UIManager.cs
    }
}
