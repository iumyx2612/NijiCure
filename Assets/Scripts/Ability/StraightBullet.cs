using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class StraightBullet : MonoBehaviour
{
    public StraightBulletData bulletData;
    
    // Data
    private int damage;
    private float range;
    private float speed;
    private int hitLimit;
    private SpriteRenderer spriteRenderer;
    private AudioClip onFireAudio;

    
    // Bullet movement and direction
    // Some stuff like defaultPos or direction is set when the bullet is enable
    // That's why we need 2 variable, cus the referencing ones always changing according to player
    [SerializeField] private Rigidbody2D bulletRb; // The RigidBody to perform physics (fire)
    [SerializeField] private Vector2Variable directionRef; // Direction to shoot 
    [SerializeField] private Vector2Variable defaultPosRef; // Start position to shoot from
    [SerializeField] private Vector2 defaultPos; // The position when the bullet was fired from (to disable bullet if too far) 
    private Vector2 bulletDirection; // Direction to shoot
    
    // Const
    private float fixedDeltaTime;
    // Rotate the bullet sprite according to the direction
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
        bulletRb = gameObject.GetComponent<Rigidbody2D>();
        directionRef.Value = Vector2.right;
        fixedDeltaTime = Time.fixedDeltaTime;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable()
    {
        if (bulletData != null)
            bulletData.state = AbilityBase.AbilityState.ready; // The first enabled bullet sets the state for the whole ability
        bulletDirection = directionRef.Value; // Set the direction once
        transform.position = defaultPosRef.Value; // Set the start firing pos once
        defaultPos = defaultPosRef.Value; // Register the start firing pos
        transform.rotation = Quaternion.Euler(directionMapping[bulletDirection]); // Set the rotation once
    }

    private void FixedUpdate()
    {
        bulletRb.AddForce(bulletDirection * speed * fixedDeltaTime,
            ForceMode2D.Impulse);
    }

    private void Update()
    {
        float dist = Vector2.Distance(defaultPos, transform.position);
        if (dist >= range)
        {
            ResetBullet();
        }
    }

    public void LoadBulletData(StraightBulletData data)
    {
        bulletData = data;
        damage = data.damage;
        range = data.range;
        speed = data.speed;
        hitLimit = data.hitLimit;
        spriteRenderer.sprite = data.sprite;
//        onFireAudio = data.onFireAudio;
    }

    public void UpgradeBulletData(StraightBulletData data)
    {
        damage = data.currentDamage;
        range = data.currentRange;
        speed = data.currentSpeed;
        hitLimit = data.currentHitLimit;
    }

    private void FireBullet()
    {
        bulletRb.AddForce(bulletDirection * speed * fixedDeltaTime,
            ForceMode2D.Impulse);
    }

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        bulletData.state = AbilityBase.AbilityState.cooldown; // The last bullet deactivated bullet sets the state for the ability
    }

    // Deal damage to enemies
    // Further behavior if hit enemy
//    private void OnCollisionEnter2D(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Enemy"))
//        {
////            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
//            
//        }
//        
//    }
}
