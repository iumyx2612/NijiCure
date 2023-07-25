using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(CircleCollider2D))]
public class ExpPickUp : MonoBehaviour, IPickUpItem
{
    private CircleCollider2D selfCollider;
    
    [SerializeField] private Sprite[] sprites;
    // For selecting what sprite to display
    private readonly List<int> expRanges = new List<int>
    {
        10, 19, 49, 99, 199
    };

    private SpriteRenderer spriteRenderer;
    private int expAmount;
    private Vector2 baseScale;

    [SerializeField] private IntGameEvent increaseExp; // Setup in LevelingSystem.cs

    private void Awake()
    {
        selfCollider = GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void LoadData(int _expAmount)
    {
        expAmount = _expAmount;
        spriteRenderer.sprite = SelectSpriteAccordingToExp(expAmount);
    }
    
    // Sorry this is a little bit ugly
    private Sprite SelectSpriteAccordingToExp(int _expAmount)
    {
        Sprite result;
        if (_expAmount <= expRanges[0])
        {
            result = sprites[0];
        }
        else if (_expAmount <= expRanges[1])
        {
            result = sprites[1];
        }
        else if (_expAmount <= expRanges[2])
        {
            result = sprites[2];
        }
        else if (_expAmount <= expRanges[3])
        {
            result = sprites[3];
        }
        else if (_expAmount <= expRanges[4])
        {
            result = sprites[4];
        }
        else
        {
            result = sprites[5];
        }

        return result;
    }

    private void DonePickUp()
    {
        gameObject.SetActive(false);
        increaseExp.Raise(expAmount); // Check LevelingSystem.cs
    }
}
