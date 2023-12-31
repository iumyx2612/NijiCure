using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UINavigation : MonoBehaviour
{
    public static UINavigation Instance;
    public InputSystemUIInputModule inputModule;
    [HideInInspector] public InputActionReference navigateAction;
    [HideInInspector] public InputActionReference submitAction;
    [HideInInspector] public InputActionReference cancelAction;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if (inputModule == null)
            {
                inputModule = FindObjectOfType<InputSystemUIInputModule>();
            }
            
            navigateAction = inputModule.move;
            submitAction = inputModule.submit;
            cancelAction = inputModule.cancel;

            Instance.cancelAction.action.performed += OnCancel;
            Instance.submitAction.action.performed += OnSubmit;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.Play("Click");
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.Play("Cancel");
    }

    private void OnDisable()
    {
        Instance.cancelAction.action.performed -= OnCancel;
        Instance.submitAction.action.performed -= OnSubmit;
    }
}
