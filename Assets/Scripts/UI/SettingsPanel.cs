using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using ScriptableObjectArchitecture;


[System.Serializable]
public class Settings
{
    public float sfxVolume;
    public float musicVolume;
    public int resolutionIndex;
    public bool fullScr;

    public Settings(float _sfxVolume, float _musicVolume, int _resolutionIndex, bool _fullScr)
    {
        sfxVolume = _sfxVolume;
        musicVolume = _musicVolume;
        resolutionIndex = _resolutionIndex;
        fullScr = _fullScr;
    }
}


public class SettingsPanel : MonoBehaviour
{
    // Manage the saves
    private JsonSerializer jsonSerializer = new JsonSerializer();
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private IntVariable resolutionIndex;
    [SerializeField] private BoolVariable isFullScr;

    [SerializeField] private ResolutionButton resolutionScript;
    // Start is called before the first frame update

    private void OnEnable()
    {
        UINavigation.Instance.cancelAction.action.performed += SaveSettings;
    }

    private void OnDisable()
    {
        UINavigation.Instance.cancelAction.action.performed -= SaveSettings;
    }

    private void Start()
    {
        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume.Value;
        musicVolumeSlider.value = AudioManager.Instance.musicVolume.Value;
    }

    public void SfxVolumeSlider()
    {
        AudioManager.Instance.SetSfxVolume(sfxVolumeSlider.value); 
        AudioManager.Instance.Play("Navigate");  
    }
    
    public void MusicVolumeSlider()
    {
        AudioManager.Instance.SetMusicVolume(musicVolumeSlider.value);    
        AudioManager.Instance.Play("Navigate");  
    }

    private void SaveSettings(InputAction.CallbackContext ctx)
    {
        if (jsonSerializer.SaveData("/settings.json", 
        new Settings(
            AudioManager.Instance.sfxVolume.Value,
            AudioManager.Instance.musicVolume.Value,
            resolutionIndex.Value,
            isFullScr.Value
        )))
        {
            Debug.Log("Save Settings Complete!");
        }
        else
        {
            Debug.LogError("Can't save Settings");
        }
    }
}
