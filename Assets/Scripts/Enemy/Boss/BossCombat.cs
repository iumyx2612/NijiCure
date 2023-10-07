using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : EnemyCombat
{
    private EnemyAbilityBase ability;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(BossData data)
    {
        damage = data.damage;
        enemyHealth = data.health;
        selfCollider.size = data.shapeToColliderMapping[data.shape].Item1;
        selfCollider.offset = data.shapeToColliderMapping[data.shape].Item2;
        ability = data.enemyAbility;
    }
}
