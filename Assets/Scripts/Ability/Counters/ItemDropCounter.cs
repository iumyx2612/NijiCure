using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Counter/Item Drop")]
public class ItemDropCounter : CounterBase
{
    public GameObject itemDropPrefab;
    [Range(0f, 1f)] public float dropChance;
}
