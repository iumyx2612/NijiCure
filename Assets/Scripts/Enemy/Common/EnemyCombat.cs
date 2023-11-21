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
#if UNITY_EDITOR
    [SerializeField] private EnemyData enemyData;
#endif
    
    // Data
    protected int damage;
    protected int enemyHealth;
    
    protected float attackRadius = 0.52f;
    protected float damageTime = 1f;
    protected float internalDamageTime;
    protected bool canAttack = true;
    protected bool crit;
    protected int actualDamageTaken;
    [SerializeField] protected LayerMask playerMask;
    
    // Stuff in Awake
    protected BoxCollider2D selfCollider;
    protected IBaseEnemyBehavior enemyMovement;
    
    // For counters that placed onto the Enemy
    [HideInInspector] public List<DamageBuffCounter> dmgBuffCounters = new List<DamageBuffCounter>();
    protected float counterDmgMultiplier;
    
    [Header("UI")]
    // Damage UI
    [SerializeField] protected IntGameEvent playerProcessDamage;
    [SerializeField] protected TMP_Text damageUIPopupText;
    [SerializeField] protected Color critColor = new Color(255, 221, 90, 1f);

    
    protected void Awake()
    {
        selfCollider = gameObject.GetComponent<BoxCollider2D>();
        enemyMovement = GetComponent<IBaseEnemyBehavior>();
#if UNITY_EDITOR
        if (enemyData != null)
        {
            LoadData(enemyData);
        }   
#endif
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        internalDamageTime += Time.deltaTime;
        if (internalDamageTime >= damageTime)
        {
            internalDamageTime = 0f;
            canAttack = true;
        }
        if (canAttack && enemyHealth > 0)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position,
                attackRadius, playerMask);
            foreach (Collider2D player in hitPlayers)
            {
                playerProcessDamage.Raise(damage);
            }

            canAttack = false;
        }
    }

    public void TakeDamage(int damage, float multiplier, Vector2 knockbackForce, float knockbackDur) // Will be called in Bullet scripts
    {
        if (enemyHealth > 0)
        {
            crit = multiplier > 1;
            counterDmgMultiplier = 1f;
            foreach (DamageBuffCounter counter in dmgBuffCounters)
            {
                // If the counter is set to increase damage of ALL ABILITIES
                if (!counter.singleAbility)
                    counterDmgMultiplier += counter.damageBuff;
            }
            actualDamageTaken = Mathf.RoundToInt(damage * multiplier * counterDmgMultiplier);
            // Apply knockback
            enemyMovement.KnockBack(knockbackForce, knockbackDur);
            enemyHealth -= actualDamageTaken;
            DamagePopupSequence(actualDamageTaken, crit);
            if (enemyHealth <= 0)
            {
                enemyMovement.Dead(false);
            }  
        }

    }

    public virtual void LoadData(EnemyData data)
    {
#if UNITY_EDITOR
        enemyData = data;
#endif
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
        if (damageUIPopupText.color.a == 1)
        {
            damageUIPopupText.gameObject.SetActive(true);
            Sequence textSequence = DOTween.Sequence();
            textSequence.Append(damageUIPopupText.transform.DOMoveY(basePosY + 0.3f, 0.2f));
            textSequence.Append(damageUIPopupText.transform.DOMoveY(basePosY - 0.5f, 0.2f));
            textSequence.Join(damageUIPopupText.DOFade(0f, 0.3f));
            textSequence.OnComplete(RecoverDamagePopUpTextState);
        }
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
