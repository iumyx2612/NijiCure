using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UINavigation : MonoBehaviour
{
    public static UINavigation Instance;
    public MyInputAction inputAction;
    
    
    private void Awake()
    {
        Instance = this;
        inputAction = new MyInputAction();
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

    public void DetectCancelEvent(InputAction.CallbackContext ctx)
    {
        Debug.Log("Cancel");
    }
}
