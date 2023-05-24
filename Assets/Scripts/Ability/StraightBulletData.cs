using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class holds data for bullet of type Straight Bullet
// All straight bullets share the same behavior, only their data is different
[CreateAssetMenu(menuName = "Bullet Data/Straight Bullet", fileName = "Straight Bullet")]
public class StraightBulletData : AbilityBase
{
    // Base data
    public int damage;
    public float range;
    public int maxBullet;
    public int startingBullet; // Number of bullet to fire per cast
    public float speed;
    public int hitLimit;
    public GameObject bulletPrefab;
    public Sprite sprite;
    public AudioClip onFireAudio;
    public List<StraightBulletUpgradeData> upgradeDatas;
    
    // Data that used during play (Reset when exit Scene, which can be upgraded)
    [HideInInspector] public int currentDamage;
    [HideInInspector] public float currentRange;
    [HideInInspector] public int numBullet; // Number of bullet to fire per cast
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public int currentHitLimit;
    

    // Create a BulletPool and bullets
    public override List<GameObject> Initialize()
    {
        currentLevel = 0;
        currentCooldownTime = baseCooldownTime;
        numBullet = startingBullet;
        currentDamage = damage;
        currentRange = range;
        currentSpeed = speed;
        currentHitLimit = hitLimit;

        GameObject temp = new GameObject(abilityName + "Holder");
        List<GameObject> bulletPool = new List<GameObject>();
        for (int i = 0; i < maxBullet; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, temp.transform);
            bullet.GetComponent<StraightBullet>().LoadBulletData(this);
            bulletPool.Add(bullet);
            bullet.SetActive(false); // Put the ability on Cooldown when first added
                                     // (state is managed in StraightBullet.cs)
        }

        return bulletPool; // This will be used in the AbilityManager.cs
    }
    
    // Used in AbilityManager.cs
    public override void TriggerAbility(List<GameObject> bulletPool)
    {
        for (int i = 0; i < numBullet; i++)
        {
            GameObject bullet = bulletPool[i];
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
            }
        }
    }

    public override void UpgradeAbility(List<GameObject> bulletPool)
    {
        StraightBulletUpgradeData upgradeData = upgradeDatas[currentLevel];
        // First we update the data that used during play for the whole ability
        upgradeData.ApplyUpgrade(this);
        // Then we update the data in the bullet
        foreach (GameObject bullet in bulletPool)
        {
            bullet.GetComponent<StraightBullet>().UpgradeBulletData(this);
        }
        // Increase the level
        currentLevel += 1;
    }
}
