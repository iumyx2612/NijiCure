using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TestRotation : MonoBehaviour
{
    public TMP_Text text;
    public Color newColor = new Color(255, 0, 0, 1);
    
    private void Start()
    {
        text.color = newColor;
    }
}
