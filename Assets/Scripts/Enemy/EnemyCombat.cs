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
    private IBaseEnemyBehavior enemyMovement;
    
    // Data
    private int damage;
    private int enemyHealth;
    private bool isAlive = true;
    private BoxCollider2D selfCollider;
    
    private float attackRadius = 0.52f;
    private float damageTime = 1f;
    private float internalDamageTime;
    private bool canAttack = true;
    private bool crit;
    private int actualDamageTaken;
    [SerializeField] private LayerMask playerMask;
    
    // For counters that placed onto the Enemy
    // One can have multiple type of counters
    [HideInInspector] public int numCounters = 0; // Need to be accessed by Abilities
    private float counterTimer;  
    private float internalCounterTimer;
    private Sprite counterSprite;
    
    [Header("UI")]
    // Damage UI
    [SerializeField] private IntGameEvent playerTakeDamage;
    [SerializeField] private TMP_Text damageUIPopupText;
    [SerializeField] private Color critColor = new Color(255, 221, 90, 1f);
    // Counter UI
    [SerializeField] private GameObject counterPanel;
    [SerializeField] private Image counterImage;
    [SerializeField] private TMP_Text numCounterText;
    
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

        if (numCounters > 0)
        {
            internalCounterTimer += Time.deltaTime;
            if (internalCounterTimer >= counterTimer)
            {
                numCounters = 0;
                CounterUIUpdate(false);
            }
        }
    }

    public void TakeDamage(int damage, float multiplier, Vector2 knockbackForce, float knockbackDur) // Will be called in Bullet scripts
    {
        // Is this memory efficient?
        crit = multiplier > 1;
        actualDamageTaken = (int) (damage * multiplier);
        // Apply knockback
        enemyMovement.KnockBack(knockbackForce, knockbackDur);
        enemyHealth -= actualDamageTaken;
        DamagePopupSequence(actualDamageTaken, crit);
        if (enemyHealth <= 0)
        {
            gameObject.GetComponent<IBaseEnemyBehavior>().Dead(false);
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
    
    // ------------------ Counter stuff ------------------
    public void ModifyCounter(int _numCounters, float _counterTimer, Sprite _counterSprite)
    {
        counterSprite = _counterSprite;
        numCounters += _numCounters;
        counterTimer = _counterTimer;
        // Reset timer of existing counters
        if (_numCounters > 0)
            internalCounterTimer = 0f;
        
        CounterUIUpdate(true);
    }

    private void CounterUIUpdate(bool update)
    {
        // De-active if run out of counter
        if (counterPanel.activeSelf && !update)
        {
            counterPanel.SetActive(false);
        }
        // Update the display of counter
        else if (counterPanel.activeSelf && update)
        {
            numCounterText.text = numCounters.ToString();
        }
        // Display the counter if not active
        else
        {
            counterPanel.SetActive(true);
            counterImage.sprite = counterSprite;
            numCounterText.text = numCounters.ToString();
        }
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
