using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector] public AudioSource audioSource;
    
    public AudioClip audioClip;
    public bool isSfx;
    public bool keep; // Keep the audio alive throughout scenes
    public string audioName;
    public bool loop;
    public bool playOnAwake;
}
