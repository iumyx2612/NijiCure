using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ExpDrop
{
    public int level;
    public Vector2 position;
    public List<float> chances;
}

namespace ScriptableObjectArchitecture
{
    // When an ExpPick is dropped, we need to have 2 things:
    // - The position of where to drop (will be where the enemy is killed)
    // - What type of ExpPick to drop
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "ExpDropGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "ExpDrop")]
    public class ExpDropGameEvent : GameEventBase<ExpDrop>
    {
        
    }
}
