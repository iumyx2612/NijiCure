using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The Counter<T> is used by Enemies
/// </summary>
/// <typeparam name="T"></typeparam>
public interface Counter<T>
{
    void SetData(T counterData);
    void Active(Vector2 position);
}

/// <summary>
/// The Data is used in Ability, and to parse data to Counter<T>
/// In depth explanation in other Counter scripts
/// </summary>
public abstract class CounterBaseData : ScriptableObject
{
    public string counterName;
    public int maxNum;
    public float existTime; 
    [HideInInspector] public float internalTime;
    [HideInInspector] public int currentNum;
}
