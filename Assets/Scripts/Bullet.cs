using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Bullet : MonoBehaviour
{
    // Bullet stats (Will be moved into ScriptableObject Data)
    [SerializeField] private FloatVariable bulletSpeed;
    [SerializeField] private FloatVariable bulletRange;
    
    // Bullet movement and direction
    // Some stuff like defaultPos or direction is set when the bullet is enable
    // That's why we need 2 variable, cus the referencing ones always changing according to player
    [SerializeField] private Rigidbody2D bulletRb; // The RigidBody to perform physics (fire)
    [SerializeField] private Vector2Variable bulletDirectionRef; // Direction to shoot 
    [SerializeField] private Vector2Variable defaultPosRef; // Start position to shoot from
    [SerializeField] private Vector2 defaultPos; // The position when the bullet was fired from (to disable bullet if too far) 
    private Vector2 bulletDirection; // Direction to shoot
    
    // Yes the mapping for direction, same purpose as the indicator mapping
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
    
    // Const
    private float fixedDeltaTime;

    private void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        bulletDirectionRef.Value = Vector2.right;
    }

    private void OnEnable()
    {
        bulletDirection = bulletDirectionRef.Value; // Set the direction once
        transform.position = defaultPosRef.Value; // Set the start firing pos once
        defaultPos = defaultPosRef.Value; // Register the start firing pos
        transform.rotation = Quaternion.Euler(directionMapping[bulletDirection]); // Set the rotation once
    }

    private void Update()
    {
        if (Vector2.Distance(defaultPos, transform.position) >= bulletRange)
        {
            ResetBullet();
        }
    }

    private void FixedUpdate()
    {
        bulletRb.AddForce(bulletDirection * bulletSpeed * fixedDeltaTime,
            ForceMode2D.Impulse);
    }

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        transform.position = defaultPos;
    }
}
