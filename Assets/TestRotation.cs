using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class TestRotation : MonoBehaviour
{
    public CircleCollider2D selfCollider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Wah");
        }
    }
}
