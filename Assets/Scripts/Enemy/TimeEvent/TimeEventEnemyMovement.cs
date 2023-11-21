using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class TimeEventEnemyMovement : EnemyMovement
{
    [SerializeField] private TimeEventSpawnDataBase timeEventEnemyData;
    
    // Data
    private float lifeTime;
    private float internalLifeTime;
    private Vector2 destination;
    private bool oneTime;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Huge performance issue
        enemyDropScript = GetComponent<EnemyDrop>();
        if (timeEventEnemyData != null)
        {
            LoadData(timeEventEnemyData, Vector2.zero);
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

    public override void Dead(bool outOfLifeTime)
    {
        if (!outOfLifeTime)
            enemyDropScript.Drop(timeEventEnemyData.expAmount);
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(OnDeadComplete);
    }

    private new void OnDeadComplete()
    {
        // Update the UI Kill info
        stageKillAmount.Value += 1;
        updateKillInfo.Raise();
        transform.parent.gameObject.SetActive(false);
        transform.localPosition = new Vector2(0, 0);
        internalLifeTime = 0f;
    }

    public void LoadData(TimeEventSpawnDataBase _data, Vector2 startPos)
    {
        timeEventEnemyData = _data;
        speed = _data.speed;
        lifeTime = _data.lifeTime;
        animator.runtimeAnimatorController = _data.runtimeAnimatorController;
        oneTime = _data.oneTime;
        if (oneTime)
        {
            destination = startPos + _data.destination;
        }
    }
}
