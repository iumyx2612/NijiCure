using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Ability/Ban Mai/Ngong")]
public class NgongData : PassiveAbilityBase
{
    [Header("Normal")]
    public Sprite counterSprite; // Sprite to display on the enemy
    [Range(0, 1)] public float damageIncrease;
    [Range(0, 1)] public float placeChance;
    public float counterDuration;
    public GameObject counterPrefab;
    public AnimationClip animationClip;
    private List<GameObject> counterPool;

    public GameObjectCollection hetBulletPool;
    
    public List<NgongData> upgradeDatas;

    [HideInInspector] public float currentDamageIncrease;
    [HideInInspector] public float currentPlaceChance;
    [HideInInspector] public float currentCounterDuration;

    public override void Initialize()
    {
        internalCooldownTime = 0f;
        currentCooldownTime = cooldownTime;
        
        currentDamageIncrease = damageIncrease;
        currentPlaceChance = placeChance;
        currentCounterDuration = counterDuration;
        
        // Init the Counter
        GameObject counterHolder = new GameObject(abilityName + " Counter");
        for (int i = 0; i < 20; i++)
        {
            GameObject counter = Instantiate(counterPrefab, counterHolder.transform);
            counter.GetComponent<CounterObject>().AddAnim(animationClip, "Ngong"); 
            counterPool.Add(counter);
            counter.SetActive(false);
        }
        
        foreach (GameObject bullet in hetBulletPool)
        {
            bullet.GetComponent<Het>().LoadNgongData(this);
        }
    }

    public override void AddAndLoadComponent(GameObject objectToAdd) {}

    public override void UpgradeAbility()
    {
        NgongData upgradeData = upgradeDatas[currentLevel];
        // Update current
        currentCounterDuration = upgradeData.counterDuration;
        currentDamageIncrease = upgradeData.damageIncrease;
        currentPlaceChance = upgradeData.placeChance;
        // Apply the upgrade 
        foreach (GameObject bullet in hetBulletPool)
        {
            bullet.GetComponent<Het>().LoadNgongData(this);
        }
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
