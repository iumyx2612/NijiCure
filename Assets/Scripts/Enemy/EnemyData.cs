using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed; 
    public int damage; 
    public int health;
    public List<float> expDropChances;
    public Sprite sprite;
}
