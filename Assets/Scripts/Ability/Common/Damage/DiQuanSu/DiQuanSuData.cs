using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Shoot bullets horizontal
/// Bullet explodes on first target hit
/// Explosion deals damage for surrounding enemies
/// </summary>
public class DiQuanSuData : DamageAbilityBase
{
    public float radiusScale;
    public int NumBullets;
    public List<bool> goBackward;

    public GameObject bulletPrefab;
    private List<GameObject> bulletPool;

    public List<DiQuanSuData> upgradeDatas;

    [HideInInspector] public float currentRadiusScale;
    [HideInInspector] public float currentNumBullets;


    public override void Initialize()
    {
        base.Initialize();

        currentNumBullets = 1;
        currentRadiusScale = 1;

        GameObject holder = new GameObject("DiQuanSuHolder");
        for (int i = 0; i < 2; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, holder.transform);
            bullet.GetComponent<DiQuanSu>().LoadData(this, goBackward[i]);
            bulletPool.Add(bullet);
            bullet.SetActive(false);
        }
    }

    public override void TriggerAbility()
    {
    }

    public override void UpgradeAbility()
    {
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsMaxLevel()
    {
        throw new System.NotImplementedException();
    }

    public override void ModifyDamage(float percentage, bool increase)
    {
        throw new System.NotImplementedException();
    }
}
