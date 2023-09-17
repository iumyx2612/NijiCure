using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class Dumb : MonoBehaviour
{
#if UNITY_EDITOR
    public DumbData testData;
#endif

    private IntGameEvent increaseExp;
    private IntGameEvent healPlayer;

    private float radius;
    private LayerMask enemyMask;
    private int numTaken;
    private int healAmount;
    private int damage;

    private int internalNumTaken;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
#if UNITY_EDITOR
        if (testData != null)
            LoadData(testData);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Trigger()
    {
        internalNumTaken += 1;
        if (internalNumTaken > numTaken)
        {
            // Heal player
            healPlayer.Raise(healAmount);
            // Deal damage
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,
                radius, enemyMask);
            for (int i = 0; i < hitEnemies.Length; i++)
            {
                hitEnemies[i].GetComponent<EnemyCombat>().TakeDamage(damage,
                    1f, Vector2.zero, 0f);
            }

            internalNumTaken = 0;
        }
    }

    public void LoadData(DumbData data)
    {
        if (increaseExp == null)
        {
            increaseExp = data.increaseExp;
            increaseExp.AddListener(Trigger);
        }

        if (healPlayer == null)
        {
            healPlayer = data.healPlayer;
        }

        radius = data.radius;
        enemyMask = data.enemyMask;
        numTaken = data.currentNumTaken;
        healAmount = data.currentHealAmount;
        damage = data.currentExplosiveDmg;
    }
}
