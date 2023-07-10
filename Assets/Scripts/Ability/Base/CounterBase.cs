using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CounterBase : ScriptableObject
{
    public string counterName;
    public int maxNum;
    public float existTime;
    [HideInInspector] public float internalTime;
    [HideInInspector] public int currentNum;
}
