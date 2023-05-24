using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bullet Data/ Circular Area Damage")]
public class CircularAreaDamageAbilityData : AbilityBase
{
    public float effectRadius;
    public float rangeFromPlayer;
    public int damage;
    public int maxNumArea;
    public int initialNumArea;
    public float existingTime;
    public GameObject bulletPrefab;
    
    // Runtime data
    [HideInInspector] public int currentNumArea;
    [HideInInspector] public float currentEffectRadius;
    [HideInInspector] public float currentRangeFromPlayer;

    public override List<GameObject> Initialize()
    {
        currentNumArea = initialNumArea;
        currentEffectRadius = effectRadius;
        currentRangeFromPlayer = rangeFromPlayer;
        
        GameObject temp = new GameObject(abilityName + "Holder");
        List<GameObject> bulletPool = new List<GameObject>();
        for (int i = 0; i < maxNumArea; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, temp.transform);
            bullet.GetComponent<CircularAreaDamageAbility>().LoadBulletData(this);
            bulletPool.Add(bullet);
            bullet.SetActive(false);
        }

        return bulletPool; // This will be used in the AbilityManager.cs
    }

    public override void TriggerAbility(List<GameObject> bulletPool)
    {
        throw new System.NotImplementedException();
    }

    public override void UpgradeAbility(List<GameObject> bulletPool)
    {
        throw new System.NotImplementedException();
    }
}
