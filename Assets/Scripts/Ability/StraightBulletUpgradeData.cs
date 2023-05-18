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

    public void ApplyUpgrade(StraightBulletData data)
    {
        base.ApplyUpgrade(data);
        // Damage 
        float newDamage = straightBulletUpgrade.percentDamage * data.currentDamage / 100;
        data.currentDamage = (int) newDamage + data.currentDamage;
        // Num Bullet
        int newNumBullet = straightBulletUpgrade.numBulletAdded + data.numBullet;
        data.numBullet = newNumBullet;
        // Range
        float newRange = straightBulletUpgrade.percentRangeAdded * data.currentRange / 100;
        data.currentRange = newRange + data.currentRange;
        // Speed
        float newSpeed = straightBulletUpgrade.percentSpeedAdded * data.currentSpeed / 100;
        data.currentSpeed = newSpeed + data.currentSpeed;
        // Hit Limit
        int newHitLimit = straightBulletUpgrade.hitLimitAdded + data.currentHitLimit;
        data.currentHitLimit = newHitLimit;
    }
}
