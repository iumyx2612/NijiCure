using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathHelper;
using ScriptableObjectArchitecture;

/// <summary>
/// Mỗi x(s) -> cooldown sẽ xuất hiện x bụi rậm ngẫu nhiên gần player
/// Ở gần bụi rậm sẽ được hồi x máu mỗi x (s) và kĩ năng cơ bản tăng x% dmg
/// Ở xa bụi rậm sẽ bị làm chậm x%
/// </summary>
/// 
[CreateAssetMenu(menuName = "Ability/Common/Passive/Guerilla")]
public class GuerillaData : PassiveAbilityBase
{
    [Header("Guerilla")]
    [Range(0, 4), SerializeField] private int numBush;
    [SerializeField] private int healAmount;
    [SerializeField] private float healTimer;
    public float existTime; // Un-upgradeable
    public float radius; // Un-upgradeable
    [Range(0f, 1f), SerializeField] private float buffPercent;
    [SerializeField] private PlayerData stagePlayerData;
    public GameObject bushPrefab;
    private List<GameObject> bushPool = new List<GameObject>();
    [SerializeField] private Vector2 spawnSpace;
    [SerializeField] private Vector2Reference playerPosRef;
    [Header("Debuff")]
    [SerializeField] private MoveSpeedCounter counter;

    [Header("Upgrades")]
    public List<GuerillaData> upgradeDatas;

    [HideInInspector] public int currentNumBush;
    [HideInInspector] public int currentHealAmount;
    [HideInInspector] public float currentHealTimer;
    [HideInInspector] public float currentBuffPercent;
    [HideInInspector] public MoveSpeedCounter currentCounter;
    [HideInInspector] public DamageAbilityBase startingAbility;


    public override void Initialize()
    {
        base.Initialize();
        state = AbilityState.cooldown;
        bushPool.Clear();

        currentNumBush = numBush;
        currentHealAmount = healAmount;
        currentHealTimer = healTimer;
        currentBuffPercent = buffPercent;
        currentCounter = new MoveSpeedCounter(counter);
        startingAbility = stagePlayerData.startingAbility as DamageAbilityBase;

        // Spawn Bush Pool
        GameObject holder = new GameObject("Bush Holder");
        for (int i = 0; i < 4; i++)
        {
            GameObject bush = Instantiate(bushPrefab, holder.transform);
            bush.GetComponent<GuerillaBush>().LoadData(this);
            bushPool.Add(bush);
            bush.SetActive(false);
        }
    }

    // Spawn Bush
    public override void TriggerAbility()
    {
        for (int i = 0; i < currentNumBush; i++)
        {
            GameObject bush = bushPool[i];
            if (!bush.activeSelf)
            {
                Vector2 position = PositionSampling.RandomPositionInSquare(
                    playerPosRef.Value, spawnSpace
                );
                bush.SetActive(true);
                bush.transform.position = position;
            }
        }   
    }

    public override void UpgradeAbility()
    {
        GuerillaData upgradeData = upgradeDatas[currentLevel];

        // Update current
        currentNumBush = upgradeData.numBush;
        currentBuffPercent = upgradeData.buffPercent;
        currentHealAmount = upgradeData.healAmount;
        currentHealTimer = upgradeData.healTimer;
        currentCounter = new MoveSpeedCounter(upgradeData.counter);

        // Apply upgrade
        for (int i = 0; i < bushPool.Count; i++)
        {
            bushPool[i].GetComponent<GuerillaBush>().LoadData(this);
        }

        currentLevel += 1;
    }

    public override void AddAndLoadComponent(GameObject objectToAdd) {}

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
