using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Linh Lan/Ultimate")]
public class UltimateHoiThoCuaDaData : UltimateAbilityBase
{
    public float activeTime;
    public float cooldownReduction;
    public float damageMultiplier;
    public float knifeScaleMultiplier;
    public float knifeDistanceMultiplier;
    public float knifeSpeedMultiplier;

    public AbilityCollection currentAbilites;

    public override void AddAndLoadUltimate(GameObject player)
    {
        player.AddComponent<UltimateHoiThoCuaDa>();
        player.GetComponent<UltimateHoiThoCuaDa>().LoadData(this);
    }
}
