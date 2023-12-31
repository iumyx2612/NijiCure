using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Nếu không có enemies ở trong radius x, sát thương kĩ năng sẽ được tăng lên x%
/// Nếu có enemies ở trong sẽ bị slow
/// </summary>
[CreateAssetMenu(menuName = "Ability/Common/Passive/Forever Alone")]
public class ForeverAloneData : PassiveAbilityBase
{
    [Range(0f, 1f)] public float damageBuff;
    public float radius;
    public MoveSpeedCounter counter;

    [SerializeField] private GameObject prefab;
    private GameObject _object;

    public List<ForeverAloneData> upgradeDatas;
    
    [HideInInspector] public float currentDmgBuff;
    [HideInInspector] public float currentRadius;
    [HideInInspector] public MoveSpeedCounter currentCounter;

    public override void Initialize()
    {
        base.Initialize();

        currentDmgBuff = damageBuff;
        currentRadius = radius;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _object = Instantiate(prefab);
        _object.transform.parent = player.transform;
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
        currentCounter = upgradeData.counter;
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
