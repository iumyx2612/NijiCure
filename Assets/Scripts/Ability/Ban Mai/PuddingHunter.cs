using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class PuddingHunter : MonoBehaviour
{
    private CircleCollider2D selfCollider;
    
    // Data
    private float radius;
    
    private void Awake()
    {
        selfCollider = GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
    }

    // Place ONE ItemDropCounter onto an Enemy
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyCounter script = collider.GetComponent<EnemyCounter>();
            
        }
    }

    public void LoadData(PuddingHunterData _data)
    {
        radius = _data.radius;
        selfCollider.radius = radius;
    }
}
