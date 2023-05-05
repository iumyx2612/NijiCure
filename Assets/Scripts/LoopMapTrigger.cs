using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LoopMapTrigger : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2Variable playerDirectionRef;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            LoopMap();
        }
    }
    
    void LoopMap()
    {    
        // Going right and on the right side of the map
        if (playerDirectionRef.Value.x > 0 && player.transform.position.x > 0)
        {
            player.transform.position = new Vector2(
                -player.transform.position.x, player.transform.position.y);
        }
        // Going left and on the left side of the map
        else if (playerDirectionRef.Value.x < 0 && player.transform.position.x < 0)
        {
            player.transform.position = new Vector2(
                -player.transform.position.x, player.transform.position.y);
        }
    }
}
