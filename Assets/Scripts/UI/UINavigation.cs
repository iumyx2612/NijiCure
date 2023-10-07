using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play sfx when navigate
    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 temp = inputAction.UI.Navigate.ReadValue<Vector2>();
        if (temp != Vector2.zero)
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
        if (inputAction != null)
        {
            inputAction.Enable();
            inputAction.UI.Navigate.performed += OnNavigate;
            inputAction.UI.Cancel.performed += OnCancel;
            inputAction.UI.Submit.performed += OnSubmit;
        }
    }

    private void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.Disable();
            inputAction.UI.Navigate.performed -= OnNavigate;
            inputAction.UI.Cancel.performed -= OnCancel;
            inputAction.UI.Submit.performed -= OnSubmit;
        }
    }
}
