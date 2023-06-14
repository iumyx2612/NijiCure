using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public EnemyData enemyData;

    // Data
    private float speed;

    private Animator animator;
    private RuntimeAnimatorController animatorController;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Transform player;
    
    private Vector2 direction;
    private bool isFacingRight;

    private bool isAlive;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        // Set alpha back to 1 since we set it to 0 in Dead()
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
        
        isAlive = true;
        if (enemyData != null)
        {
            LoadData(enemyData);
            animator.runtimeAnimatorController = animatorController;
        }
    }

    private void Update()
    {
        if (isAlive)
        {
            // Going right but not facing right
            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            // Going left but facing right
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            direction = ((Vector2) player.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    public void LoadData(EnemyData data)
    {
        enemyData = data;
        speed = data.speed;
        spriteRenderer.sprite = data.sprite;
        animatorController = data.animatorController;
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);

        isFacingRight = !isFacingRight;
    }


    public void Dead()
    {
        gameObject.GetComponent<EnemyDrop>().Drop(enemyData.expAmount);
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(delegate { gameObject.transform.parent.gameObject.SetActive(false); });
    }
}
