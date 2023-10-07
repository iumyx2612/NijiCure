using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data/Boss Data")]
public class BossData : EnemyData
{
    [Tooltip("Only for Boss that has Ability")]
    public EnemyAbilityBase enemyAbility;
}
