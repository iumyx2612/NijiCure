using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class TimeEventEnemyMovement : MonoBehaviour, IBaseEnemyBehavior
{
    private TimeEventEnemyData enemyData;
    
    // Data
    private float speed;
    private float lifeTime;
    private float internalLifeTime;
    private Vector2 destination;
    private bool oneTime;
    private RuntimeAnimatorController animatorController;
    
    private bool isAlive;
    private bool canMove;
    private Vector2 direction;
    private bool isFacingRight;
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        isAlive = true;
        canMove = true;
        if (enemyData != null)
        {
            animator.runtimeAnimatorController = animatorController;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            internalLifeTime += Time.deltaTime;
            if (internalLifeTime >= lifeTime)
            {
                isAlive = false;
                Dead(true);
            }
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
        if (canMove)
        {
            // Constantly moving toward player
            if (!oneTime)
                direction = ((Vector2) player.position - rb.position).normalized;
            // Move to pre-defined destination
            else
                direction = (destination - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }
    
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);

        isFacingRight = !isFacingRight;
    }

    public void LoadData(TimeEventEnemyData _data)
    {
        enemyData = _data;
        speed = _data.speed;
        lifeTime = _data.lifeTime;
        spriteRenderer.sprite = _data.sprite;
        animatorController = _data.animatorController;
    }
    
    public void KnockBack(Vector2 force, float duration)
    {
        canMove = false;
        Sequence knockbackSequence = DOTween.Sequence();
        knockbackSequence.Append(rb.DOMove(rb.position - force, duration));
        knockbackSequence.OnComplete(() => { canMove = true; });
    }
    
    public void Dead(bool outOfLifeTime)
    {
        // Only drop EXP when not die by out of life time
        if (!outOfLifeTime)
            gameObject.GetComponent<EnemyDrop>().Drop(enemyData.expAmount);
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(OnDeadComplete);
    }

    private void OnDeadComplete()
    {
        transform.parent.gameObject.SetActive(false);
        transform.localPosition = new Vector2(0, 0);
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
    }
}
