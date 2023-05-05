using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Straight Bullet Upgrade")]
// This is upgrade that designed specific for StraightBullet
public class StraightBulletUpgradeData : UpgradeAbilityBase
{
    [Serializable]
    public struct StraightBulletUpgrades
    {
        public float percentDamage;
        public int numBulletAdded;
        public float percentRangeAdded;
        public float percentSpeedAdded;
        public int hitLimitAdded;
    }
    [SerializeField]
    public StraightBulletUpgrades straightBulletUpgrade;

    public new void ApplyUpgrade(StraightBulletData data)
    {
        base.ApplyUpgrade(data);
        // Damage 
        float newDamage = straightBulletUpgrade.percentDamage * data.currentDamage;
        data.currentDamage = (int) newDamage;
        // Num Bullet
        int newNumBullet = straightBulletUpgrade.numBulletAdded + data.numBullet;
        data.numBullet = newNumBullet;
        // Range
        float newRange = straightBulletUpgrade.percentRangeAdded * data.currentRange;
        data.currentRange = newRange;
        // Speed
        float newSpeed = straightBulletUpgrade.percentSpeedAdded * data.currentSpeed;
        data.currentSpeed = newSpeed;
        // Hit Limit
        int newHitLimit = straightBulletUpgrade.hitLimitAdded + data.currentHitLimit;
        data.currentHitLimit = newHitLimit;
    }
}
