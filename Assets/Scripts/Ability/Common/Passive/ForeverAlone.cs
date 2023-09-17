using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class ForeverAlone : MonoBehaviour
{
    [SerializeField] private AbilityCollection currentAbilities;
    private bool isAlone;
    private bool havingBuff;
    private float damageBuff;
    private float radius;

    private CircleCollider2D collider;

    private List<Collider2D> enemies;


    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (enemies.Count > 0)
        {
            isAlone = false;
            if (havingBuff)
            {
                for (int i = 0; i < currentAbilities.Count; i++)
                {
                    if (currentAbilities[i] is DamageAbilityBase dmgAbility)
                    {
                        dmgAbility.ModifyDamage(damageBuff, false);
                    }
                }
            }

            havingBuff = false;
        }
        else
        {
            isAlone = true;
        }
        if (!havingBuff && isAlone)
        {
            for (int i = 0; i < currentAbilities.Count; i++)
            {
                if (currentAbilities[i] is DamageAbilityBase dmgAbility)
                {
                    dmgAbility.ModifyDamage(damageBuff, true);
                }
            }

            havingBuff = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other);
        }
    }

    public void LoadData(ForeverAloneData data)
    {
        damageBuff = data.currentDmgBuff;
        radius = data.currentRadius;
        collider.radius = radius;
    }
}
