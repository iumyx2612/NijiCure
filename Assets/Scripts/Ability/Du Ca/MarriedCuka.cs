using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class MarriedCuka : MonoBehaviour
{
    // Data
    private int damage;
    private float radius; // Range
    private float attackTimer;
    private float lifeTime;
    [SerializeField] private LayerMask enemyMask;

    private float internalAttackTimer;
    private float internalLifeTime;
    private bool canAttack;

    private SpriteRenderer spriteRenderer; // The SpriteRenderer of MarriedCuka
    
    // For Li Di Ability
    [SerializeField] private BoolVariable hasLiDiAbility;
    [SerializeField] private IntGameEvent healPlayer;
    private int healAmount;
    private int explosiveDamage;
    private Animator cukaAnimator;
    private float animLength;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cukaAnimator = GetComponent<Animator>();
        animLength = cukaAnimator.GetCurrentAnimatorStateInfo(0).length;
    }

    // Update is called once per frame
    void Update()
    {
        internalLifeTime += Time.deltaTime;
        internalAttackTimer += Time.deltaTime;
        if (internalLifeTime >= lifeTime)
        {
            Dead();
            internalLifeTime = 0f;
        }

        if (canAttack)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,
                radius, enemyMask);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyCombat>().TakeDamage(
                    damage, 2f, Vector2.zero, 0f);
            }

            canAttack = false;
        }

        if (internalAttackTimer >= attackTimer)
        {
            canAttack = true;
            internalAttackTimer = 0f;
        }
        
    }

    public void LoadData(ChongQuocDanData data)
    {
        damage = data.currentCukaDamage;
        radius = data.cukaRange;
        attackTimer = data.currentCukaAtkTimer;
        lifeTime = data.cukaLifeTime;
    }

    public void LoadLiDiData(LiDiData data)
    {
        explosiveDamage = data.currentExplosiveDamage;
        healAmount = data.currentHealAmount;
    }

    private void Dead()
    {
        if (!hasLiDiAbility.Value)
        {
            Sequence deadSequence = DOTween.Sequence();
            deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
            deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
            deadSequence.OnComplete(OnDeadComplete);
        }
        else
        {
            healPlayer.Raise(healAmount);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,
                radius, enemyMask);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyCombat>().TakeDamage(
                    explosiveDamage, 2f, Vector2.zero, 0f);
            }
        }
    }

    private void OnDeadComplete()
    {
        // Set alpha back to 1 since we set it to 0 in Dead()
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
    }
}
