using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public string characterName;
    public Sprite playerIcon;
    public PlayerType type;
    public int health;
    public float speed;
    public int rank;
    [Range(0f, 1f)] public float critChance;
    public RuntimeAnimatorController animatorController;

    public AbilityBase startingAbility;
    public UltimateAbilityBase ultimateAbility;

    public Sound playerSound;

    public void Set(PlayerData _data)
    {
        playerIcon = _data.playerIcon;
        type = _data.type;
        health = _data.health;
        speed = _data.speed;
        critChance = _data.critChance;
        animatorController = _data.animatorController;
        startingAbility = _data.startingAbility;
        ultimateAbility = _data.ultimateAbility;
    }
}
