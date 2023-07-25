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

    [SerializeField] public string[] colliderTags;


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
        for (int i = 0; i < colliderTags.Length; i++)
        {
            if (other.CompareTag(colliderTags[i]))
            {
                other.GetComponent<IPickUpItem>().OnPickUp(transform);
            }
        }
    }
}
