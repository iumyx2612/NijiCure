using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


// Bullet of this class will triggers in a circular area around the player
// Note: The bullet will not follow the player
public class CircularBullet : MonoBehaviour
{
    public CircularBulletData bulletData;
    
    // Data
    private int damage;
    private float radius;
    private float circulatingTime; // The time for bullet to complete a circulation (note that circulation can be more or less than 1 circle)
    private float speed; // Speed of the bullet
    private SpriteRenderer spriteRenderer;
    private AudioClip onFireAudio;

    private Rigidbody2D rb;
    private Transform anchor; // This should be the bullet holder, which is its parent
    private Vector2 defaultPos; // Position to fire bullet from (Set once OnEnable)
    [SerializeField] private Vector2Variable defaultPosRef; // Position to fire bullet from (referencing)
    private float internalTime; // Active time


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anchor = gameObject.transform.parent;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (bulletData != null)
        {
            bulletData.state = AbilityBase.AbilityState.ready;
        }

        defaultPos = defaultPosRef.Value;
        anchor.position = defaultPos; // Set the anchor position to make bullet circulate around it
    }

    // Ability State manager
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            internalTime += Time.deltaTime;
            bulletData.state = AbilityBase.AbilityState.active;
            if (internalTime >= circulatingTime)
            {
                ResetBullet();
            }
        }
    }

    // Circulate the bullet
    private void FixedUpdate()
    {
        Circulate(speed, anchor);
    }

    public void LoadBulletData(CircularBulletData data)
    {
        bulletData = data;
        damage = data.damage;
        radius = data.radius;
        speed = data.speed;
        spriteRenderer.sprite = data.sprite;
        circulatingTime = data.circulatingTime;
//        onFireAudio = data.OnFireAudioClip;
    }
    
    // The bullet circulate around a defined position (not follow player)
    private void Circulate(float orbitSpeed, Transform anchor)
    {
        Quaternion q = Quaternion.AngleAxis (orbitSpeed * Time.fixedDeltaTime, transform.forward);
        rb.MovePosition (q * (rb.transform.position - anchor.position) + anchor.position);
        rb.MoveRotation (rb.transform.rotation * q);
    }

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        internalTime = 0;
        bulletData.state = AbilityBase.AbilityState.cooldown;
    }
}
