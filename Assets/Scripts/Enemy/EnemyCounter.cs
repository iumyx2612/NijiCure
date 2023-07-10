using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    private IBaseEnemyBehavior iBaseEnemyBehaviorScript;
    private EnemyCombat enemyCombatScript;

    // Item Drop Counters
    private List<ItemDropCounter> itemDropCounters;
    
    private void Awake()
    {
        iBaseEnemyBehaviorScript = GetComponent<IBaseEnemyBehavior>();
        enemyCombatScript = GetComponent<EnemyCombat>();
    }

    // Use LateUpdate here since Counter will be placed by Abilities during Update
    // We want to start timer countdown right after Counters are placed 
    private void LateUpdate()
    {
        throw new NotImplementedException();
    }
}
