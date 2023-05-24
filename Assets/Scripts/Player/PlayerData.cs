using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public PlayerType type;
    public int health;
    public float speed;
    public int rank;
    public float critChance;
    public RuntimeAnimatorController animatorController;

    public AbilityBase startingAbility;
}
