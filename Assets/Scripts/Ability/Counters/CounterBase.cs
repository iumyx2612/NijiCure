using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The Counter<T> is used by Enemies
/// SetData() to set the CounterData to this Counter on the Enemy
/// Active() to play the Animation of adding Counter
/// </summary>
/// <typeparam name="T"></typeparam>
/// 
[System.Serializable]
public abstract class CounterBase
{
    [Header("Base")]
    public string abilityName;
    public string counterName;
    public int maxNum;
    public float existTime;
    // For Anim
    public GameObject counterPrefab;
    [HideInInspector] public List<GameObject> counterPool;
    [HideInInspector] public GameObject counterHolder;
    
    // For Anim
    public virtual void InitCounterObject()
    {
        counterPool.Clear();
        if (counterPrefab != null)
        {
            counterHolder = new GameObject(abilityName + " Counters");
            for (int i = 0; i < 20; i++)
            {
                GameObject counter = GameObject.Instantiate(counterPrefab, counterHolder.transform);
                counterPool.Add(counter);
                counter.SetActive(false);
            }
        }
    }
    public abstract void Active(Vector2 position);
}
