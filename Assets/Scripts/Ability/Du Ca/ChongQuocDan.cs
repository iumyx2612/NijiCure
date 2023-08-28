using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChongQuocDan : MonoBehaviour
{
    private ChongQuocDanData baseData;
    
    // Data
    [SerializeField] private float radius;
    [SerializeField] private int maxNumCuka;
    [SerializeField] private GameObjectCollection cukaPool;
    private float convertChance;
    private int damage;
    
    private CircleCollider2D selfCollider;

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
        damage = data.currentDamage;
    }
}
