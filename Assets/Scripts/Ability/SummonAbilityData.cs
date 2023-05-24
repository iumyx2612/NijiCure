using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Data/Summon")]
public class SummonAbilityData : AbilityBase
{
    //Base data
    public int damage;
    public float summonRadius; // How far can summon go, given player as 
    public int maxSummons;
    public float summonSpeed;
    public float timePerAttack;
    public float summonAttackRadius;
    public Sprite sprite;
    public GameObject summonPrefab;

    // Data that used during play (Reset when exit Scene, which can be upgraded)
    [HideInInspector] public int numSummon;

    public enum SummonState
    {
        moving,
        attacking,
        cooldown
    }

    [HideInInspector] public SummonState summonState;

    public override List<GameObject> Initialize()
    {
        // Re-init runtime data
        numSummon = 1;
        
        GameObject temp = new GameObject(abilityName + "Holder");
        List<GameObject> summonPool = new List<GameObject>();
        for (int i = 0; i < maxSummons; i++)
        {
            GameObject summon = Instantiate(summonPrefab, temp.transform);
            summon.GetComponent<SummonAbility>().LoadBulletData(this);
            summonPool.Add(summon);
        }

        state = AbilityState.active; // Ability of type Summon is always active
        return summonPool;
    }
    
    // The ability is trigger through its internal bullet script: SummonAbility.cs
    public override void TriggerAbility(List<GameObject> bulletPool)
    {
        return;
    }

    public override void UpgradeAbility(List<GameObject> bulletPool)
    {
        
    }
}
