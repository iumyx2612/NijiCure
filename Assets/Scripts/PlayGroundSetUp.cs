using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Unity.VisualScripting;
using UnityEngine;

public class PlayGroundSetUp : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2Variable playerMovementRef;

    private void Update()
    {
        
    }

    private void LoopMap()
    {

        player.transform.position = new Vector2(-player.transform.position.x,
            player.transform.position.y);
        Debug.Log("Loop");
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
