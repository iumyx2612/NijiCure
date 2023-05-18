using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    [SerializeField] private EnemyEventCollection eventCollection;
    [SerializeField] private EnemySpawnGameEvent enemySpawnGameEvent;
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private FloatVariable timeSinceGameStart;
    public Camera camera;
    
    private int eventIndex;
    private Vector2 travelDirection;
    
    
    private void Awake()
    {
        enemySpawnGameEvent.AddListener(TriggerEvent);
        camera = FindObjectOfType<Camera>();
    }
    
    private void Update()
    {
        timeSinceGameStart.Value += Time.deltaTime;
        if (eventIndex >= eventCollection.Count)
        {
            return;
        }

        if (timeSinceGameStart.Value >= eventCollection[eventIndex].timeToTrigger)
        {
            enemySpawnGameEvent.Raise(eventCollection[eventIndex]);
        }
    }

    private void TriggerEvent(EnemyEventData eventData)
    {
        
    }
}
