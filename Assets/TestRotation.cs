using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngleCalculation;
using UnityEngine.EventSystems;


public class TestRotation : MonoBehaviour, ICancelHandler
{
    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log("A");
    }
}
