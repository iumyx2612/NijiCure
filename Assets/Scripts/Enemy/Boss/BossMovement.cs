using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossMovement : EnemyMovement
{
    [SerializeField] private BossSpawnerData bossData;


    public void LoadData(BossSpawnerData data)
    {
        bossData = data;
        speed = data.speed;
        animator.runtimeAnimatorController = data.runtimeAnimatorController;
    }

    public override void Dead(bool outOfLifeTime)
    {
        if (!outOfLifeTime)
            enemyDropScript.Drop(bossData.expAmount);
        Sequence deadSequence = DOTween.Sequence();
        deadSequence.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.5f));
        deadSequence.Join(spriteRenderer.DOFade(0f, 0.5f));
        deadSequence.OnComplete(OnDeadComplete);
    }
}
