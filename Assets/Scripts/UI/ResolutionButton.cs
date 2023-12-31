using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;

public class ResolutionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Reference")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text optionText;
    [SerializeField] private BoolVariable isFullScr;
    [SerializeField] private IntVariable resolutionIndex;

    [Space]
    [SerializeField] private int index;
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

    private void Awake()
    {
        index = resolutionIndex.Value;
    }

    private void Start()
    {
        RefreshShownValue();
    }

    private void RefreshShownValue()
    {
        if (options != null)
            optionText.text = options[index];
    }

    private void ChangeResolution()
    {
        Vector2Int resolution = resolutions[index];
        resolutionIndex.Value = index;
        Screen.SetResolution(resolution.x, resolution.y, isFullScr.Value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    public void ChangeScreenMode(bool _isFullscr)
    {
        isFullScr.Value = _isFullscr;
        Screen.fullScreen = isFullScr.Value;
    }

    private void OnRightNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 temp = UINavigation.Instance.navigateAction.action.ReadValue<Vector2>();
        if (temp == Vector2.right)
        {
            if ((index + 1) >= options.Count)
            {
                index = 0;
            }
            else
            {
                index++;
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
            if (index == 0)
            {
                index = options.Count - 1;
            }
            else
            {
                index -= 1;
            }
            RefreshShownValue();
            ChangeResolution();
        }
    }
}
