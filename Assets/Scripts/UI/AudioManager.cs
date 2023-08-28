using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;
    public static AudioManager Instance;

    [SerializeField] private FloatVariable sfxVolume;
    [SerializeField] private FloatVariable musicVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();

            sound.audioSource.clip = sound.audioClip;
            if (sound.isSfx)
                sound.audioSource.volume = sfxVolume.Value;
            else
                sound.audioSource.volume = musicVolume.Value;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = sound.playOnAwake;
        }
        SetMusicVolume(musicVolume.Value);
        SetSfxVolume(sfxVolume.Value);

        for (int i = 0; i < sounds.Count; i++)
        {
            Sound sound = sounds[i];
            if (sound.playOnAwake)
            {
                sound.audioSource.Play();
            }
        }
    }

    public void Play(string name)
    {
        Sound s = sounds.Find(sound => sound.audioName == name);
        if(s == null)
        {
            return;
        }
        s.audioSource.Play();
    }

    public void Mute(string name)
    {
        Sound s = sounds.Find(sound => sound.audioName == name);
        if(s == null)
        {
            return;
        }
        s.audioSource.volume = 0f;
    }

    public void UnMute(string name)
    {
        Sound s = sounds.Find(sound => sound.audioName == name);
        if(s == null)
        {
            return;
        }
        s.audioSource.volume = s.volume;
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.audioName == name);
        if (s == null)
            return;
        s.audioSource.Stop();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume.Value = value;
        foreach (Sound sound in sounds)
        {
            if (sound.isSfx)
            {
                sound.audioSource.volume = sfxVolume.Value;
            }
        }
    }

    public void SetMusicVolume(float value)
    {
        musicVolume.Value = value;
        foreach (Sound sound in sounds)
        {
            if (!sound.isSfx)
            {
                sound.audioSource.volume = musicVolume.Value;
            }
        }
    }

    public void AddSound(Sound sound)
    {
        List<string> allSoundNames = new List<string>();
        for (int i = 0; i < sounds.Count; i++)
        {
            allSoundNames.Add(sounds[i].audioName);
        }
        if (!allSoundNames.Contains(sound.audioName)
        && sound.audioClip != null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();

            sound.audioSource.clip = sound.audioClip;
            if (sound.isSfx)
                sound.audioSource.volume = sfxVolume.Value;
            else
                sound.audioSource.volume = musicVolume.Value;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = sound.playOnAwake;
            
            sounds.Add(sound);
        }
    }

    public void MassRemoveSound()
    {
        for (int i = sounds.Count - 1; i >= 0; i--)
        {
            if (!sounds[i].keep)
            {
                sounds.RemoveAt(i);
            }
        }
    }
}
