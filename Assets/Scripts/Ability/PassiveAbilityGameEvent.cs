using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PassiveAbilityInfo
{
    public float duration;
    public Sprite UISprite;

    public PassiveAbilityInfo(float _duration, Sprite _UISprite)
    {
        duration = _duration;
        UISprite = _UISprite;
    }
}


namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(menuName = "Ability/Passive Ability Game Event")]
    public class PassiveAbilityGameEvent : GameEventBase<PassiveAbilityInfo>
    {
        
    }
}

