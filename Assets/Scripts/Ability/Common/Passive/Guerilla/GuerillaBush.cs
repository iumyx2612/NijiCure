using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(CircleCollider2D))]
public class GuerillaBush : MonoBehaviour
{
    private GuerillaData baseData;
    private CircleCollider2D selfCollider;

    // Data
    private float existTime;
    private int healAmount;
    private float healTimer;
    private float buffPercent;
    private MoveSpeedCounter counter;
    private DamageAbilityBase startingAbility;
    private PlayerCounter counterScript;
    [SerializeField] private IntGameEvent healPlayer;

    // State management
    [SerializeField] private float internalExistTime;
    [SerializeField] private float internalHealTimer;
    private bool inRange;

    
    private void Awake()
    {
        counterScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCounter>();
        selfCollider = GetComponent<CircleCollider2D>();
        selfCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        internalExistTime += Time.deltaTime;
        if (internalExistTime >= existTime)
        {
            internalExistTime = 0f;
            gameObject.SetActive(false);
        }
        internalHealTimer += Time.deltaTime;
        if (internalHealTimer >= healTimer)
        {
            internalHealTimer = 0f;
            if (inRange)
            {
                healPlayer.Raise(healAmount);
            }
        }
    }

    private void OnEnable()
    {        
        if (counter != null) // Avoid the init in Pool
        {
            inRange = false;
            if (counterScript.GetNumMoveSpdCounter(counter.counterName) < 1)
                counterScript.AddMoveSpdCounter(counter);
        }
    }

    private void OnDisable()
    {
        internalHealTimer = 0f;
        baseData.state = AbilityBase.AbilityState.cooldown;
        baseData.internalCooldownTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = true;
            counterScript.RemoveMoveSpdCounter(counter.counterName);
            startingAbility.ModifyDamage(buffPercent, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = false;
            counterScript.AddMoveSpdCounter(counter);
            startingAbility.ModifyDamage(buffPercent, false);
        }
    }

    public void LoadData(GuerillaData data)
    {
        if (baseData == null)
        {
            baseData = data;
            startingAbility = data.startingAbility;
            selfCollider.radius = data.radius;
            existTime = data.existTime;
        }
        healAmount = data.currentHealAmount;
        healTimer = data.currentHealTimer;
        buffPercent = data.currentBuffPercent;
        counter = data.currentCounter;
    }
}
