using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player/Mapping")]
public class PlayerMapping : ScriptableObject
{
    [HideInInspector] public PlayerType playerType;
    [HideInInspector] public AbilityBase startingAbility;
    [HideInInspector] public float critChance; 
}
