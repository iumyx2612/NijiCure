using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "AbilityGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "Ability")]
    public class AbilityGameEvent : GameEventBase<AbilityBase>
    {
        
    }
}
