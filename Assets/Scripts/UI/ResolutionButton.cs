using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class ResolutionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Reference")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text optionText;

    [Space]
    [SerializeField] private int value;
    [SerializeField] private List<string> options = new List<string> 
    {
        "1920 x 1080", "1600 x 900", "1270 x 720"
    };
    [SerializeField] private List<Vector2Int> resolutions = new List<Vector2Int>
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),
        new Vector2Int(1270, 720)
    };
    [SerializeField] private bool isFullscr = true;


    public void OnSelect(BaseEventData eventData)
    {
        UINavigation.Instance.navigateAction.action.performed += OnRightNavigate;
        UINavigation.Instance.navigateAction.action.performed += OnLeftNavigate;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        UINavigation.Instance.navigateAction.action.performed -= OnRightNavigate;
        UINavigation.Instance.navigateAction.action.performed -= OnLeftNavigate;
    }

    private void Start()
    {
        RefreshShownValue();
    }

    private void RefreshShownValue()
    {
        if (options != null)
            optionText.text = options[value];
    }

    private void ChangeResolution()
    {
        Vector2Int resolution = resolutions[value];
        Screen.SetResolution(resolution.x, resolution.y, isFullscr ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    public void ChangeScreenMode(bool _isFullscr)
    {
        isFullscr = _isFullscr;
        Screen.fullScreen = isFullscr;
    }

    private void OnRightNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 temp = UINavigation.Instance.navigateAction.action.ReadValue<Vector2>();
        if (temp == Vector2.right)
        {
            if ((value + 1) >= options.Count)
            {
                value = 0;
            }
            else
            {
                value++;
            }
            RefreshShownValue();
            ChangeResolution();
        }
    }

    private void OnLeftNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 temp = UINavigation.Instance.navigateAction.action.ReadValue<Vector2>();
        if (temp == Vector2.left)
        {
            if (value == 0)
            {
                value = options.Count - 1;
            }
            else
            {
                value -= 1;
            }
            RefreshShownValue();
            ChangeResolution();
        }
    }
}
