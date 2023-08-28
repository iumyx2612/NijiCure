using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This shares the Dead method between Normal Enemy Movement
/// and Time Event Enemy
/// since both Enemy type use the same EnemyCombat.cs but different Movement script
/// </summary>
public interface IBaseEnemyBehavior
{
    void Dead(bool outOfLifeTime);
    void KnockBack(Vector2 force, float duration);
    void Flip();
    void ModifySpdCounter(List<MoveSpeedCounter> counters);
    void ModifySpeed(float percentage, bool toBase); // toBase: return to normal speed
}