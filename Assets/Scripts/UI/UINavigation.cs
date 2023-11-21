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
    [SerializeField] private BoolVariable navigationControl;
    [HideInInspector] public InputActionReference navigateAction;
    [HideInInspector] public InputActionReference submitAction;
    [HideInInspector] public InputActionReference cancelAction;
    
    private void Awake()
    {
        Instance = this;
        navigationControl.Value = true; // Initial load, always true
        if (inputModule == null)
        {
            inputModule = FindObjectOfType<InputSystemUIInputModule>();
        }
        navigateAction = inputModule.move;
        submitAction = inputModule.submit;
        cancelAction = inputModule.cancel;
    }

    // Play sfx when navigate
    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 temp = Instance.navigateAction.action.ReadValue<Vector2>();
        if (temp != Vector2.zero && navigationControl.Value != false)
            AudioManager.Instance.Play("Navigate");
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.Play("Click");
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.Play("Cancel");
    }

    private void OnEnable()
    {
        Instance.navigateAction.action.performed += OnNavigate;
        Instance.cancelAction.action.performed += OnCancel;
        Instance.submitAction.action.performed += OnSubmit;
    }

    private void OnDisable()
    {
        Instance.navigateAction.action.performed -= OnNavigate;
        Instance.cancelAction.action.performed -= OnCancel;
        Instance.submitAction.action.performed -= OnSubmit;
    }
}
