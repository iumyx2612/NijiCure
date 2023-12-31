using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenuUI : MonoBehaviour
{
    // Manage save
    private JsonSerializer jsonSerializer = new JsonSerializer();
    [Header("Where to load saved data to")]
    [SerializeField] private IntVariable resolutionIndex;
    [SerializeField] private BoolVariable isFullScr;
    [SerializeField] private FloatVariable sfxVolume;
    [SerializeField] private FloatVariable musicVolume;

    private List<Vector2Int> resolutions = new List<Vector2Int>
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),
        new Vector2Int(1270, 720)
    };


    private void Awake()
    {
        // Try to load from saved
        try
        {
            Settings savedSettings = jsonSerializer.LoadData<Settings>("/settings.json");
            
            resolutionIndex.Value = savedSettings.resolutionIndex;
            Vector2Int resolution = resolutions[resolutionIndex.Value];
            isFullScr.Value = savedSettings.fullScr;
            Screen.SetResolution(resolution.x, resolution.y,
             isFullScr.Value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

            sfxVolume.Value = savedSettings.sfxVolume;
            musicVolume.Value = savedSettings.musicVolume;
        }
        // If no saved
        catch(Exception)
        {
            Screen.SetResolution(Screen.currentResolution.width, 
            Screen.currentResolution.height, 
            FullScreenMode.FullScreenWindow);
            sfxVolume.Value = 1f;
            musicVolume.Value = 1f;
        }

    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
