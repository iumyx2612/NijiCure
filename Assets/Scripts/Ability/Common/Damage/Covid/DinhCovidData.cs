using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using MathHelper;

/// <summary>
/// This is a summon Ability
/// Summon a region of Covid which moves every x seconds
/// Region of Covid deals damage every x seconds whoever stands inside
/// </summary>
/// 
[CreateAssetMenu(menuName = "Ability/Common/Damage/Dinh Covid")]
public class DinhCovidData : DamageAbilityBase
{
    [Tooltip("Damage tick, the cooldownTime is time for the region to move")]
    public float timeToDamage;
    [Tooltip("Counter Data to attach slow to enemy")]
    public MoveSpeedCounter counter;
    public float radiusScale;
    public Vector2 moveArea; // Unupgrade-able
    public GameObject covidPrefab;
    public Vector2Variable playerPosRef; // Move around player

    public List<DinhCovidData> upgradeDatas;

    [HideInInspector] public float currentTimeToDamage;
    [HideInInspector] public float currentRadiusScale;
    [HideInInspector] public MoveSpeedCounter currentCounter;

    public override void Initialize()
    {
        base.Initialize();

        currentTimeToDamage = timeToDamage;
        currentRadiusScale = 1;
        currentCounter = new MoveSpeedCounter(counter);

        // Set data to 
        covidPrefab = Instantiate(covidPrefab);
        covidPrefab.GetComponent<DinhCovid>().LoadData(this);

    }

    // This uses to move the Covid region
    public override void TriggerAbility()
    {
        Vector2 newPosition = PositionSampling.RandomPositionInSquare(playerPosRef.Value, moveArea);
        covidPrefab.GetComponent<DinhCovid>().SetNewPosition(newPosition);
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
