using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed; 
    public int damage; 
    public int health;
    public int expAmount;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
}
