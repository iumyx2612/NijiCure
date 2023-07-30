using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class Rage : MonoBehaviour
{
    // Data
    private float buffTime;
    private float damageBuff;
    private Vector2 scaleBuff;
    private HetData baseHetData;
    private IntGameEvent playerTakeDamage;

    private bool havingBuff;
    private float internalBuffTime;

    // UI
    private Sprite abilityIcon;
    private PassiveAbilityGameEvent activeCountdownImage; // Setup in UIManager.cs
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (havingBuff)
        {
            internalBuffTime += Time.deltaTime;
            if (internalBuffTime >= buffTime)
            {
                internalBuffTime = 0f;
                havingBuff = false;
                baseHetData.ModifyDamage(damageBuff, false);
            }
        }
    }

    private void Buff()
    {
        activeCountdownImage.Raise(new PassiveAbilityInfo(buffTime, abilityIcon));
        internalBuffTime = 0f;
        if (!havingBuff)
            baseHetData.ModifyDamage(damageBuff, true);
        havingBuff = true;
    }

    public void LoadData(RageData _data)
    {
        buffTime = _data.currentBuffTime;
        damageBuff = _data.currentDamageBuff;
        scaleBuff = _data.currentScaleBuff;
        if (baseHetData == null)
            baseHetData = _data.baseHetData;
        if (playerTakeDamage == null)
        {
            playerTakeDamage = _data.playerTakeDamage;
            playerTakeDamage.AddListener(Buff);
        }
        if (activeCountdownImage == null)
        {
            abilityIcon = _data.abilityIcon;
            activeCountdownImage = _data.activeCountdownImage;
        }
    }
    
}
