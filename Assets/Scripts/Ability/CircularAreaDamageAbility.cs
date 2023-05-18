using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D))]
public class CircularAreaDamageAbility : MonoBehaviour
{
    public CircularAreaDamageAbilityData bulletData;

    // Data
    private float effectRadius;
    private float rangeFromPlayer;
    private int damage;
    private float existingTime;
    
    // Runtime 
    private Vector2 position;
    private Vector2 anchorPos;
    [SerializeField] private Vector2Variable playerPosRef;
    private float internalExistingTime;

    private Rigidbody2D rb;
    private CircleCollider2D selfCollider;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        selfCollider = gameObject.GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bulletData != null)
        {
            LoadBulletData(bulletData);
        }
    }

    private void OnEnable()
    {
        selfCollider.radius = effectRadius;
        anchorPos = playerPosRef.Value;
        position = SamplePosition(anchorPos, rangeFromPlayer);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            internalExistingTime += Time.deltaTime;
            if (internalExistingTime >= existingTime)
            {
                ResetBullet();
            }
        }
    }

    private Vector2 SamplePosition(Vector2 anchor, float range)
    {
        float center_x = anchor.x;
        float center_y = anchor.y;
        float min_x = center_x - range;
        float max_x = center_x + range;
        float min_y = center_y - range;
        float max_y = center_y + range;
        float pos_x = Random.Range(min_x, max_x);
        float pos_y = Random.Range(min_y, max_y);
        Vector2 newPos = new Vector2(pos_x, pos_y);
        return newPos;
    }

    public void LoadBulletData(CircularAreaDamageAbilityData data)
    {
        effectRadius = data.effectRadius;
        rangeFromPlayer = data.rangeFromPlayer;
        damage = data.damage;
        existingTime = data.existingTime;
    }

    private void ResetBullet()
    {
        internalExistingTime = 0f;
        gameObject.SetActive(false);
        bulletData.state = AbilityBase.AbilityState.cooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Wah");
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
