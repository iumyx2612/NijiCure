using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ChongQuocDan : MonoBehaviour
{
    private ChongQuocDanData baseData;
    
    // Data
    [SerializeField] private GameObjectCollection cukaPool;
    private float convertChance;
    private int damage;

    private Vector2 baseScale;
    private EdgeCollider2D selfCollider;
    
    // State management
    private Animator animator;
    private float animLength;
    private float internalLength;
    
    
    private void Awake()
    {
        selfCollider = GetComponent<EdgeCollider2D>();
        animator = GetComponent<Animator>();
        animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        selfCollider.enabled = false;
    }

    private void Update()
    {
        internalLength += Time.deltaTime;
        if (internalLength >= animLength)
        {
            ResetBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= convertChance)
            {
                other.GetComponent<IBaseEnemyBehavior>().Dead(true);
                // Grab inactive married cuka from pool
                for (int i = 0; i < cukaPool.Count; i++)
                {
                    GameObject marriedCuka = cukaPool[i];
                    if (!marriedCuka.activeSelf)
                    {
                        marriedCuka.SetActive(true);
                        marriedCuka.transform.position = other.transform.position;
                        break;
                    }
                }
            }
            // Deal damage
            else
            {
                other.GetComponent<EnemyCombat>().TakeDamage(damage, 1f, Vector2.zero, 0f);
            }
        }
    }

    public void LoadData(ChongQuocDanData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }

        convertChance = data.currentConvertChance;
        if (data.currentZoneRadius > 1)
        {
            Vector2 newScale = baseScale * data.currentZoneRadius;
            transform.localScale = newScale;
        }
        damage = data.currentDamage;
    }
    
    private void ResetBullet()
    {
        internalLength = 0f;
        gameObject.SetActive(false);
        baseData.state = AbilityBase.AbilityState.cooldown; // The last deactivated bullet sets the state for the ability
    }

    private void EnableCollider() // Used in Animation
    {
        selfCollider.enabled = true;
    }

    private void DisableCollider() // Used in Animation
    {
        selfCollider.enabled = false;
    }
}
