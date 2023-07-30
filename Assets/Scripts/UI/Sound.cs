using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioSource audioSource;
    
    public AudioClip audioClip;
    public string audioName;
    public bool loop;
    public bool playOnAwake;
    [Range(0f, 1f)] public float volume;
}
