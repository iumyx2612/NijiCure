using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed; 
    public int damage; 
    public int health;
    public int expAmount;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    public enum Shape
    {
        horizontal,
        vertical,
        square
    }

    public Shape shape;
    [HideInInspector] public Dictionary<Shape, (Vector2, Vector2)> shapeToColliderMapping = 
        new Dictionary<Shape, (Vector2, Vector2)>
        {
            {Shape.horizontal, (new Vector2(0.33f, 0.2f), new Vector2(0, -0.06f))},
            {Shape.vertical, (new Vector2(0.24f, 0.13f), new Vector2(0, -0.08f))},
            {Shape.square, (new Vector2(0.24f, 0.22f), new Vector2(0, -0.08f))}
        };
}
