using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class TestRotation : MonoBehaviour
{
    public InputSystemUIInputModule inputModule; 

    private InputActionReference navigateAction;
    private InputActionReference submitAction;
    private void Awake()
    {
        if (inputModule == null)
        {
            inputModule = FindObjectOfType<InputSystemUIInputModule>();
        }
        navigateAction = inputModule.move;
        submitAction = inputModule.submit;
        navigateAction.action.performed += OnNavigate;
        submitAction.action.performed += OnSubmit;
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 temp = navigateAction.action.ReadValue<Vector2>();
        if (temp != Vector2.zero)
            AudioManager.Instance.Play("Navigate");
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.Play("Click");
    }
}
