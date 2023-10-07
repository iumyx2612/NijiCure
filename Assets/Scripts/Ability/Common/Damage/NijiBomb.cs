using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NijiBomb : MonoBehaviour
{
    private NijiBombData baseData;
    // Data
    private int damage;
    private float critChance;
    private float multiplier;
    private float explosiveRadius;

    // Components
    private CircleCollider2D selfCollider;
    private Animator animator; // To play the exploding animation

    // State management
    private float internalLength;
    private float animLength;

    private void Awake()
    {
        selfCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        animLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    // Update is called once per frame
    void Update()
    {
        internalLength += Time.deltaTime;
        if (internalLength >= animLength)
        {
            ResetBullet();
        }
    }
    
    public void LoadData(NijiBombData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }
        damage = data.currentDamage;
        critChance = data.currentCritChance;
        explosiveRadius = data.currentExplosiveRadius;
        selfCollider.radius = explosiveRadius;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            float critRandom = Random.Range(0f, 1f);
            if (critRandom <= critChance)
            {
                multiplier = 1f;
            }
            else
            {
                multiplier = 1.5f;
            }
            collider.GetComponent<EnemyCombat>().TakeDamage(damage,
            multiplier, Vector2.zero, 0f);
        }
    }

    private void EnableCollider() // Used in Animation
    {
        selfCollider.enabled = true;
    }

    private void DisableCollider() // Used in Animation
    {
        selfCollider.enabled = false;
    }

    private void ResetBullet()
    {
        internalLength = 0f;
        gameObject.SetActive(false);
        baseData.state = AbilityBase.AbilityState.cooldown; // The last deactivated bullet sets the state for the ability
    }
}
