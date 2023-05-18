using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "ListFloatGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "ListFloat")]
    public class ListFloatGameEvent : GameEventBase<List<float>>
    {
    
    }
}

