using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenuUI : MonoBehaviour
{
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    

    public void SfxVolumeSlider()
    {
        AudioManager.Instance.SetSfxVolume(sfxVolumeSlider.value);    
    }
    
    public void MusicVolumeSlider()
    {
        AudioManager.Instance.SetMusicVolume(musicVolumeSlider.value);    
    }
}
