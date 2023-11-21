using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class NijiBomb : MonoBehaviour
{
    private NijiBombData baseData;
    // Data
    private int damage;
    private float critChance;
    private float multiplier;
    private Vector2 baseScale;

    private Vector2 offset = new Vector2(0, 0.6f);
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject bombGameObject;


    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.position = (Vector2)bombGameObject.transform.position + offset;
    }

    public void LoadData(NijiBombData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }
        damage = data.currentDamage;
        critChance = data.currentCritChance;
        if (data.radiusScale > 1)
        {
            Vector2 newScale = baseScale * data.radiusScale;
            transform.localScale = newScale;
            offset *= data.radiusScale;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            float critRandom = Random.Range(0f, 1f);
            if (critRandom <= critChance)
            {
                multiplier = 2f;
            }
            else
            {
                multiplier = 1f;
            }
            collider.GetComponent<EnemyCombat>().TakeDamage(
                damage, multiplier, Vector2.zero, 0f
            );
        }
    }

    private void ResetBullet()
    {
        holder.SetActive(false);
        gameObject.SetActive(false);
        bombGameObject.SetActive(true);
        baseData.state = AbilityBase.AbilityState.cooldown; // The last deactivated bullet sets the state for the ability
    }

}
