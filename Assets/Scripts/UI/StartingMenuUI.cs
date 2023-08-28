using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class StartingMenuUI : MonoBehaviour
{
    [SerializeField] private Slider sfxVolumeSlider;
    

    public void SfxVolumeSlider()
    {
        AudioManager.Instance.SetSfxVolume(sfxVolumeSlider.value);    
    }
}
