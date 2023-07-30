using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    public static AudioManager _instance;

    private void Awake()
    {
        _instance = this;
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();

            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = sound.playOnAwake;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        if(s == null)
        {
            return;
        }
        s.audioSource.Play();
    }

    public void Mute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.volume = 0f;
    }

    public void UnMute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.volume = s.volume;
    }
}
