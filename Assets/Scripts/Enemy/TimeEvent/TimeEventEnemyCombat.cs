using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEventEnemyCombat : EnemyCombat
{
    public void LoadData(TimeEventSpawnDataBase data)
    {
        damage = data.damage;
        enemyHealth = data.health;
        selfCollider.size = data.shapeToColliderMapping[data.shape].Item1;
        selfCollider.offset = data.shapeToColliderMapping[data.shape].Item2;
    }
}
