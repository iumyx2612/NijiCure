using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class LanKnife : MonoBehaviour
{
    private LanKnifeData baseData;
    
    // Data
    private int damage;
    private float knifeSpeed;
    private float knifeDistance;
    private float knifeScale;
    private float critChance;
    private float multiplier;

    private Rigidbody2D rb;
    private Vector2 defaultScale;
    
    // Bullet movement and direction
    // Some stuff like defaultPos or direction is set when the bullet is enable
    // That's why we need 2 variable, cus the referencing ones always changing according to player
    [SerializeField] private Vector2Variable directionRef; // Direction to shoot 
    [SerializeField] private Vector2Variable defaultPosRef; // Start position to shoot from
    private Vector2 defaultPos; // The position when the bullet was fired from (to disable bullet if too far) 
    public Vector2 bulletDirection; // Direction to shoot
    
    private readonly Dictionary<Vector2, Vector3> directionMapping = 
        new Dictionary<Vector2, Vector3>
        {
            {Vector2.right, new Vector3(0, 0, 0)},
            {Vector2.left, new Vector3(0, 180, 0)},
            {Vector2.up, new Vector3(0, 0, 90)},
            {Vector2.down, new Vector3(0, 0, -90)},
            {new Vector2(-1, 1), new Vector3(0, 180, 45)},
            {new Vector2(-1, -1), new Vector3(0, 180, -45)},
            {new Vector2(1, 1), new Vector3(0, 0, 45)},
            {new Vector2(1, -1), new Vector3(0, 0, -45)},
            {Vector2.zero, new Vector3(0, 0, 0)}
        };


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = new Vector2(defaultScale.x * knifeScale, defaultScale.y * knifeScale);
        bulletDirection = directionRef.Value; // Set the direction once
        transform.position = defaultPosRef.Value; // Set the start firing pos once
        defaultPos = defaultPosRef.Value; // Register the start firing pos
        transform.rotation = Quaternion.Euler(directionMapping[bulletDirection]); // Set the rotation once
    }

    private void Update()
    {
        float dist = Vector2.Distance(defaultPos, transform.position);
        if (dist >= knifeDistance)
        {
            ResetBullet();
        }
    }

    private void FixedUpdate()
    {
        FireBullet();
    }

    public void LoadData(LanKnifeData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }
        // Things can changes during runtime
        damage = data.currentDamage;
        knifeSpeed = data.currentKnifeSpeed;
        knifeDistance = data.currentKnifeDistance;
        knifeScale = data.currentKnifeScale;
        critChance = data.currentCritChance;
    }

    private void FireBullet()
    {
        rb.AddForce(bulletDirection * knifeSpeed * Time.fixedDeltaTime,
            ForceMode2D.Impulse);
    }
    
    private void ResetBullet()
    {
        gameObject.SetActive(false);
        baseData.state = AbilityBase.AbilityState.cooldown; // The last deactivated bullet sets the state for the ability
    }

    // Deal damage to enemies
    // Further behavior if hit enemy
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= critChance)
            {
                multiplier = 1.5f;
            }
            else
            {
                multiplier = 1f;
            }
            collider.GetComponent<EnemyCombat>().TakeDamage(damage, multiplier,
                Vector2.zero, 0);
        }
    }
}
