using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player/Mapping")]
public class PlayerTypeAndStartingAbility : ScriptableObject
{
    public PlayerType playerType;
    public AbilityBase startingAbility;
}
