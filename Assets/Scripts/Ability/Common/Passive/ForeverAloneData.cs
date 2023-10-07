using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Nếu không có enemies ở trong radius x, sát thương kĩ năng sẽ được tăng lên x%
/// </summary>
[CreateAssetMenu(menuName = "Ability/Common/Passive/Forever Alone")]
public class ForeverAloneData : PassiveAbilityBase
{
    public float damageBuff;
    public float radius;

    [SerializeField] private GameObject prefab;
    private GameObject _object;

    public List<ForeverAloneData> upgradeDatas;
    
    [HideInInspector] public float currentDmgBuff;
    [HideInInspector] public float currentRadius;

    public override void Initialize()
    {
        base.Initialize();

        currentDmgBuff = damageBuff;
        currentRadius = radius;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _object = Instantiate(prefab, player.transform);
        AddAndLoadComponent(_object);
    }

    public override void AddAndLoadComponent(GameObject objectToAdd)
    {
        objectToAdd.GetComponent<ForeverAlone>().LoadData(this);
    }

    public override void UpgradeAbility()
    {
        ForeverAloneData upgradeData = upgradeDatas[currentLevel];

        currentRadius = upgradeData.radius;
        currentDmgBuff = upgradeData.damageBuff;
        _object.GetComponent<ForeverAlone>().LoadData(this);

        currentLevel += 1;
    }

    public override bool IsMaxLevel()
    {
        if (currentLevel >= upgradeDatas.Count && currentLevel >= 1)
        {
            return true;
        }

        return false;
    }

    public override AbilityBase GetUpgradeDataInfo()
    {
        return upgradeDatas[currentLevel];
    }
}
