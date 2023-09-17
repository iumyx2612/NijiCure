using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UINavigation : MonoBehaviour
{
    public static UINavigation Instance;
    public DefaultInputActions inputAction;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inputAction = new DefaultInputActions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }
}
