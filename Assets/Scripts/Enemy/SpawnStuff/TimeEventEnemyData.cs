using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;


[CreateAssetMenu(menuName = "Enemy/Enemy Data/Time Event")]
public class TimeEventEnemyData : EnemyData
{
    /// <summary>
    /// Already has:
    ///    public float speed; 
    ///    public int damage; 
    ///    public int health;
    ///    public int expAmount;
    ///    public Sprite sprite;
    ///    public RuntimeAnimatorController animatorController;
    /// </summary>
    // Setup by TimeEventSpawnData ScriptableObject
    [HideInInspector] public Vector2 destination;
    [HideInInspector] public float lifeTime;
    [HideInInspector] public bool oneTime;
}
