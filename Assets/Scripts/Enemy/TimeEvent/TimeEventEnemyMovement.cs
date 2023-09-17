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
    
    // Cache
    private EnemyDrop enemyDropScript;
    
    [SerializeField] private Transform enemyUICanvas;
    
    // Counter
    private List<MoveSpeedCounter> spdCounters;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Huge performance issue
        enemyDropScript = GetComponent<EnemyDrop>();
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
        enemyUICanvas.position = transform.position;
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
        destination = _data.destination;
        oneTime = _data.oneTime;
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
            enemyDropScript.Drop(enemyData.expAmount);
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
    
    public void ModifySpdCounter(List<MoveSpeedCounter> counters)
    {
        spdCounters = new List<MoveSpeedCounter>(counters);
        float percentage = 0f;
        for (int i = 0; i < spdCounters.Count; i++)
        {
            MoveSpeedCounter counter = spdCounters[i];
            // TODO: Is this math correct?
            if (counter.increase)
                percentage += counter.percentage;
            else
                percentage -= counter.percentage;
        }
        ModifySpeed(percentage, false);
    }
    
    public void ModifySpeed(float percentage, bool toBase)
    {
        if (!toBase)
        {
            speed = (percentage + 1) * speed;
        }
        else
        {
            speed = speed / (1 + percentage);
        }
    }
}