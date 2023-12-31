using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PassiveAbilityInfo
{
    public float duration;
    public Sprite UISprite;
    public int stacks;
    public bool isStatic;
    public bool reset;

    public PassiveAbilityInfo(float _duration, Sprite _UISprite, int _stacks, bool _isStatic, bool _reset)
    {
        duration = _duration;
        UISprite = _UISprite;
        stacks = _stacks;
        isStatic = _isStatic;
        reset = _reset;
    }
}


namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Ability/Passive Ability Game Event")]
    public class PassiveAbilityGameEvent : GameEventBase<PassiveAbilityInfo>
    {
        
    }
}

