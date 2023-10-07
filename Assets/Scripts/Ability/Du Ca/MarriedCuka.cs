using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

[RequireComponent(
    typeof(SpriteRenderer),
    typeof(Animator),
    typeof(CircleCollider2D))]
public class MarriedCuka : MonoBehaviour
{
    // Data
    private int damage = 20;
    private float radius; // Range
    private float attackTimer = 0.8f;
    private float lifeTime = 150f;

    private CircleCollider2D selfCollider;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    private float internalAttackTimer;
    private float internalLifeTime;
    private bool canAttack;

    private SpriteRenderer spriteRenderer; // The SpriteRenderer of MarriedCuka
    
    // For Li Di Ability
    private bool hasLiDiAbility;
    [SerializeField] private IntGameEvent healPlayer;
    private int healAmount;
    private int explosiveDamage;
    private Animator cukaAnimator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cukaAnimator = GetComponent<Animator>();
        selfCollider = GetComponent<CircleCollider2D>();
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
            for (int i = 0; i < hitEnemies.Count; i++)
            {
                hitEnemies[i].GetComponent<EnemyCombat>().TakeDamage(
                    damage, 1.0f, Vector2.zero, 0f);
            }

            canAttack = false;
        }

        if (internalAttackTimer >= attackTimer)
        {
            canAttack = true;
            internalAttackTimer = 0f;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemies.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemies.Remove(other);
        }
    }

    public void LoadData(ChongQuocDanData data)
    {
        damage = data.currentCukaDamage;
        radius = data.cukaRange;
        selfCollider.radius = radius;
        attackTimer = data.currentCukaAtkTimer;
        lifeTime = data.cukaLifeTime;
    }

    public void LoadLiDiData(LiDiData data)
    {
        hasLiDiAbility = true;
        explosiveDamage = data.currentExplosiveDamage;
        healAmount = data.currentHealAmount;
    }

    private void Dead()
    {
        if (hasLiDiAbility)
        {            
            healPlayer.Raise(healAmount);
            for (int i = 0; i < hitEnemies.Count; i++)
            {
                hitEnemies[i].GetComponent<EnemyCombat>().TakeDamage(
                    explosiveDamage, 2.0f, Vector2.zero, 0f);
            }
        }
        
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(OnDeadComplete);
    }

    private void OnDeadComplete()
    {
        gameObject.SetActive(false);
        // Set alpha back to 1 since we set it to 0 in Dead()
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
    }
}
