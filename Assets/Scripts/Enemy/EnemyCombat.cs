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
    private BoxCollider2D selfCollider;
    
    private float attackRadius = 0.52f;
    private float damageTime = 1f;
    private float internalDamageTime;
    private bool canAttack = true;
    [SerializeField] private LayerMask playerMask;
    
    [Header("UI")]
    [SerializeField] private IntGameEvent playerTakeDamage;
    [SerializeField] private TMP_Text damageUIPopupText;
    

    private void Awake()
    {
        selfCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        if (enemyData != null)
        {
            LoadData(enemyData);
        }
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
    }

    private void FixedUpdate()
    {
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

    public void TakeDamage(int damage) // Will be called in Bullet scripts
    {
        enemyHealth -= damage;
        DamagePopupSequence(damage);
        if (enemyHealth <= 0)
        {
            gameObject.GetComponent<EnemyMovement>().Dead();
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

    private void DamagePopupSequence(int damage)
    {
        damageUIPopupText.text = damage.ToString();
        GameObject damageUIPopupGO = damageUIPopupText.gameObject;
        damageUIPopupGO.transform.position = transform.position;
        float basePosY = damageUIPopupGO.transform.position.y;
        damageUIPopupGO.SetActive(true);
        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(damageUIPopupGO.transform.DOMoveY(basePosY + 0.3f, 0.3f));
        textSequence.Append(damageUIPopupGO.transform.DOMoveY(basePosY - 0.5f, 0.5f));
        textSequence.Join(damageUIPopupText.DOFade(0f, 0.7f));
        textSequence.OnComplete(RecoverDamagePopUpTextState);
    }

    private void RecoverDamagePopUpTextState()
    {
        damageUIPopupText.gameObject.SetActive(false);
        Color temp = damageUIPopupText.color;
        temp.a = 1;
        damageUIPopupText.color = temp;
    }
}
