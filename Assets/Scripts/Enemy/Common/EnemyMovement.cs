using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour, IBaseEnemyBehavior
{
    [SerializeField] private EnemyData enemyData;

    // Data
    protected float speed;
    
    protected Vector2 direction;
    protected bool isFacingRight;
    protected bool isAlive;
    protected bool canMove;

    // Using GetComponent at Awake
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected Transform player;
    protected EnemyDrop enemyDropScript;
    
    // UI stuff
    [SerializeField] protected Transform enemyUICanvas;
    [SerializeField] protected IntVariable stageKillAmount;
    [SerializeField] protected GameEvent updateKillInfo;
    
    // Speed counter
    protected List<MoveSpeedCounter> spdCounters;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Performance issue
        enemyDropScript = GetComponent<EnemyDrop>();
        if (enemyData != null)
        {
            LoadData(enemyData);
        }
    }

    private void OnEnable()
    {
        isAlive = true;
        canMove = true;
        // Set alpha back to 1 since we set it to 0 in Dead()
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
    }

    private void Update()
    {
        enemyUICanvas.position = transform.position;
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
        if (canMove)
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
        animator.runtimeAnimatorController = data.animatorController;
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);

        isFacingRight = !isFacingRight;
    }

    public void KnockBack(Vector2 force, float duration)
    {
        canMove = false;
        Sequence knockbackSequence = DOTween.Sequence();
        knockbackSequence.Append(rb.DOMove(rb.position - force, duration));
        knockbackSequence.OnComplete(() => { canMove = true; });
    }

    public virtual void Dead(bool outOfLifeTime)
    {
        if (!outOfLifeTime)
            enemyDropScript.Drop(enemyData.expAmount);
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(OnDeadComplete);
    }

    protected void OnDeadComplete()
    {
        // Update the UI Kill info
        stageKillAmount.Value += 1;
        updateKillInfo.Raise();
        transform.parent.gameObject.SetActive(false);
        transform.localPosition = new Vector2(0, 0);
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
