using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "AbilityGameEvent.asset",
        menuName = "Ability/Ability Game Event")]
    public class AbilityGameEvent : GameEventBase<AbilityBase>
    {
        
    }
}
