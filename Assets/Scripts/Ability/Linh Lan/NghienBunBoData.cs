using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Linh Lan/Nghien Bun Bo")]
// When Linh Lan eats, she will gain damage buff for x seconds
public class NghienBunBoData : PassiveAbilityBase
{
    public int damageIncrease;
    public float duration;

    public List<NghienBunBoData> upgradeDatas;

    
    public override void UpgradeAbility()
    {
        NghienBunBoData upgradeData = upgradeDatas[currentLevel];
        
    }

    public override void AddAndLoadComponent(GameObject player)
    {
        player.AddComponent<NghienBunBo>();
        player.GetComponent<NghienBunBo>().LoadData(this);
    }
}
