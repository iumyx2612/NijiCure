using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;


[RequireComponent(typeof(CircleCollider2D))]
public class PlayerPickUp : MonoBehaviour
{
    public FloatVariable pickUpRadius;
    
    private CircleCollider2D pickUpCollider;


    private void Awake()
    {
        pickUpCollider = gameObject.GetComponent<CircleCollider2D>();
        pickUpCollider.isTrigger = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        pickUpCollider.radius = pickUpRadius.Value;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PickUp") || other.CompareTag("Heal"))
        {
            other.GetComponent<IPickUpItem>().OnPickUp(transform);
        }
    }
}
