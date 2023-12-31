using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class DiQuanSu : MonoBehaviour
{
    // Data
    private DiQuanSuData baseData;
    private int damage;
    private float critChance;
    private float multiplier;
    private float baseRadius;
    private bool goBackward;
    [SerializeField] private float maxDist;
    [SerializeField] private float bulletSpeed;

    // References
    private Animator animator;
    private CircleCollider2D explosionCollider;
    private BoxCollider2D bulletCollider;
    private Rigidbody2D rb;
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private Vector2Variable directionRef;
    private Vector2 oldDirection;
    private Vector2 direction;
    [SerializeField] private Vector2 basePosition;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        explosionCollider = GetComponent<CircleCollider2D>();
        bulletCollider = GetComponent<BoxCollider2D>();
        direction = Vector2.right;
        baseRadius = explosionCollider.radius;
    }

    private void OnEnable()
    {
        transform.position = basePosition = playerPosRef.Value;
        direction = new Vector2(directionRef.Value.x, 0);
        if (goBackward)
            direction = new Vector2(-directionRef.Value.x, 0);
        if (direction.x != 0)
        {
            oldDirection = direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(rb.position, basePosition) >= maxDist)
        {
            Explode();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + oldDirection * bulletSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= critChance)
            {
                multiplier = 2f;
            }
            else
            {
                multiplier = 1f;
            }
            collider.GetComponent<EnemyCombat>().TakeDamage(damage, multiplier, Vector2.zero, 0f);
        }
    }

    private void Explode()
    {
        bulletCollider.enabled = false;
        explosionCollider.enabled = true;
    }

    public void ResetBullet() // Use in Animation Event
    {
        gameObject.SetActive(false);
        bulletCollider.enabled = true;
        explosionCollider.enabled = false;
    }

    public void LoadData(DiQuanSuData data, bool _goBackward)
    {
        if (baseData == null)
        {
            baseData = data;
        }
        damage = data.currentDamage;
        critChance = data.currentCritChance;
        if (data.currentRadiusScale > 1)
        {
            float newRadius = baseRadius * data.currentRadiusScale;
            explosionCollider.radius = newRadius;
        }
        goBackward = _goBackward;
    }
}
