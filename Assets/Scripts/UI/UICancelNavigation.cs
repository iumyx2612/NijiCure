using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

/// <summary>
/// Put this on a main Panel which composes of other UI elements like Button, Sliders, etc...
/// </summary>
public class UICancelNavigation : MonoBehaviour
{
    public GameObject previousPanel;
    [Tooltip("Go back to previous Scene")]
    [SerializeField] private bool goBack; 
    [Tooltip("Check if SetActive(true) the gameobject when OnCancel is called")]
    [SerializeField] private bool deact;
    [SerializeField] private Selectable startingGameObject;

    private void OnEnable()
    {
        if (startingGameObject != null)
        {
            startingGameObject.Select();
        }

        if (UINavigation.Instance != null)
            UINavigation.Instance.cancelAction.action.performed += OnCancel;
    }

    private void OnDisable()
    {
        if (UINavigation.Instance != null)
            UINavigation.Instance.cancelAction.action.performed -= OnCancel;
    }

    public void OnCancel(InputAction.CallbackContext ctx)
    {
        if (deact)
        {
            gameObject.SetActive(false);
        }
        if (previousPanel != null && !goBack)
        {
            previousPanel.SetActive(true);
        }
        // Go back to previous Scene
        else if (previousPanel == null && goBack)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIndex - 1);
        }
    }

    public void SetPrevPanel(GameObject prevPanel)
    {
        previousPanel = prevPanel;
    }
}
