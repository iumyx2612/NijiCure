using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Data/Circular Bullet (static)")]
public class CircularBulletData : AbilityBase
{
    public int damage;
    public float radius;
    public float speed;
    public float circulatingTime;
    public int maxBullet;
    public int numBullet;
    public GameObject bulletPrefab;
    public Sprite sprite;
    public AudioClip OnFireAudioClip;

    public override List<GameObject> Initialize(GameObject player)
    {
        GameObject temp = new GameObject(name + "Holder");
        List<GameObject> bulletPool = new List<GameObject>();
        for (int i = 0; i < maxBullet; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, temp.transform);
            Vector2 bulletPos = new Vector2(temp.transform.position.x + radius, 
                temp.transform.position.y);
            bullet.transform.position = bulletPos; 
            bullet.GetComponent<CircularBullet>().LoadBulletData(this);
            bulletPool.Add(bullet);
            bullet.SetActive(false);
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
                Debug.Log("Active Circle");
            }
        }
    }

    public override void UpgradeAbility(List<GameObject> bulletPool, int tier)
    {
        
    }
}
