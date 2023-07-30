using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCombat : MonoBehaviour
{
    public EnemyData enemyData;
    
    // Data
    private int damage;
    private int enemyHealth;
    private bool isAlive = true;
    
    private float attackRadius = 0.52f;
    private float damageTime = 1f;
    private float internalDamageTime;
    private bool canAttack = true;
    private bool crit;
    private int actualDamageTaken;
    [SerializeField] private LayerMask playerMask;
    
    // Stuff in Awake
    private BoxCollider2D selfCollider;
    private IBaseEnemyBehavior enemyMovement;
    
    // For counters that placed onto the Enemy
    [HideInInspector] public List<DamageBuffCounter> dmgBuffCounters = new List<DamageBuffCounter>();
    private float counterDmgMultiplier;
    
    [Header("UI")]
    // Damage UI
    [SerializeField] private IntGameEvent playerTakeDamage;
    [SerializeField] private TMP_Text damageUIPopupText;
    [SerializeField] private Color critColor = new Color(255, 221, 90, 1f);

    
    private void Awake()
    {
        selfCollider = gameObject.GetComponent<BoxCollider2D>();
        enemyMovement = GetComponent<IBaseEnemyBehavior>();
        if (enemyData != null)
        {
            LoadData(enemyData);
        }
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        internalDamageTime += Time.deltaTime;
        if (internalDamageTime >= damageTime)
        {
            internalDamageTime = 0f;
            canAttack = true;
        }
        if (canAttack && isAlive)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position,
                attackRadius, playerMask);
            foreach (Collider2D player in hitPlayers)
            {
                playerTakeDamage.Raise(damage);
            }

            canAttack = false;
        }
    }

    public void TakeDamage(int damage, float multiplier, Vector2 knockbackForce, float knockbackDur) // Will be called in Bullet scripts
    {
        crit = multiplier > 1;
        counterDmgMultiplier = 1f;
        foreach (DamageBuffCounter counter in dmgBuffCounters)
        {
            // If the counter is set to increase damage of ALL ABILITIES
            if (!counter.singleAbility)
                counterDmgMultiplier += counter.damageBuff;
        }
        actualDamageTaken = (int) (damage * multiplier * counterDmgMultiplier);
        // Apply knockback
        enemyMovement.KnockBack(knockbackForce, knockbackDur);
        enemyHealth -= actualDamageTaken;
        DamagePopupSequence(actualDamageTaken, crit);
        if (enemyHealth <= 0)
        {
            enemyMovement.Dead(false);
        }
    }

    public void LoadData(EnemyData data)
    {
        enemyData = data;
        damage = data.damage;
        enemyHealth = data.health;
        selfCollider.size = data.shapeToColliderMapping[data.shape].Item1;
        selfCollider.offset = data.shapeToColliderMapping[data.shape].Item2;
    }
    
    // ------------------ UI Animation ------------------

    private void DamagePopupSequence(int damage, bool crit)
    {
        damageUIPopupText.text = damage.ToString();
        if (crit)
        {
            damageUIPopupText.text = $"{damage}!";
            damageUIPopupText.color = critColor;
            damageUIPopupText.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        damageUIPopupText.transform.position = transform.position;
        float basePosY = transform.position.y;
        damageUIPopupText.gameObject.SetActive(true);
        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(damageUIPopupText.transform.DOMoveY(basePosY + 0.3f, 0.3f));
        textSequence.Append(damageUIPopupText.transform.DOMoveY(basePosY - 0.5f, 0.5f));
        textSequence.Join(damageUIPopupText.DOFade(0f, 0.7f));
        textSequence.OnComplete(RecoverDamagePopUpTextState);
    }

    private void RecoverDamagePopUpTextState()
    {
        damageUIPopupText.gameObject.SetActive(false);
        damageUIPopupText.transform.localScale = new Vector2(1f, 1f);
        // Change color back to white (since we change it if crit)
        Color temp = damageUIPopupText.color;
        temp.a = 1;
        temp.r = 255;
        temp.g = 255;
        temp.b = 255;
        damageUIPopupText.color = temp;
    }
}
