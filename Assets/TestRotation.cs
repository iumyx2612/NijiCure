using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;


public class TestRotation : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 basePos;
    
    private void OnEnable()
    {
       rb.velocity = Vector2.right;
    }

    private void Update()
    {
//        if (Vector2.Distance(basePos, transform.position) >= 2f)
//        {
//            gameObject.SetActive(false);
//        }
    }

    private void FixedUpdate()
    {
//        rb.MovePosition(rb.position + Vector2.right * Time.fixedDeltaTime * 5);
    }
}
