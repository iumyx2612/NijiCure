using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour, IBaseEnemyBehavior
{
    public EnemyData enemyData;

    // Data
    private float speed;
    private RuntimeAnimatorController animatorController;
    
    private Vector2 direction;
    private bool isFacingRight;
    private bool isAlive;
    private bool canMove;

    // Using GetComponent at Awake
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;
    private EnemyDrop enemyDropScript;
    
    // UI stuff
    [SerializeField] private Transform enemyUICanvas;
    [SerializeField] private IntVariable stageKillAmount;
    [SerializeField] private GameEvent updateKillInfo;


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
        if (enemyData != null)
        {
            animator.runtimeAnimatorController = animatorController;
        }
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
        animatorController = data.animatorController;
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

    public void Dead(bool outOfLifeTime)
    {
        enemyDropScript.Drop(enemyData.expAmount);
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(OnDeadComplete);
    }

    private void OnDeadComplete()
    {
        // Update the UI Kill info
        stageKillAmount.Value += 1;
        updateKillInfo.Raise();
        transform.parent.gameObject.SetActive(false);
        transform.localPosition = new Vector2(0, 0);
        // Set alpha back to 1 since we set it to 0 in Dead()
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
    }
}
