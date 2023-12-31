using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    [SerializeField] private List<Sound> tempSounds = new List<Sound>();
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    
    public FloatVariable sfxVolume;
    public FloatVariable musicVolume;
    
    private void Awake()
    {
        //Singleton method
        if (Instance == null) 
        {
            //First run, set the instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.tempSounds = new List<Sound>();
            for (int i = 0; i < tempSounds.Count; i++)
            {
                Instance.tempSounds.Add(tempSounds[i]);
            }
            Destroy(gameObject);
        }
        // Remove sound from previous Scene
        for (int i = Instance.sounds.Count - 1; i >= 0; i--)
        {
            Sound sound = Instance.sounds[i];
            if (!sound.keep)
            {
                Instance.sounds.Remove(sound);
            }
        }

        for (int i = 0; i < Instance.tempSounds.Count; i++)
        {
            AddSound(Instance.tempSounds[i]);
        }
        Instance.tempSounds.Clear();
    }

    private void Start()
    {
        SetMusicVolume(musicVolume.Value);
        SetSfxVolume(sfxVolume.Value);
        for (int i = 0; i < Instance.sounds.Count; i++)
        {
            Sound sound = Instance.sounds[i];
            if (sound.playOnAwake)
            {
                sound.audioSource.Play();
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Instance.sounds.Find(sound => sound.audioName == name);
        if(s == null)
        {
            return;
        }
        s.audioSource.Play();
    }

    public void Stop(string name)
    {
        Sound s = Instance.sounds.Find(sound => sound.audioName == name);
        if (s == null)
            return;
        s.audioSource.Stop();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume.Value = value;
        Instance.audioMixer.SetFloat("SfxVolume", Mathf.Log10(sfxVolume.Value) * 20);

    }

    public void SetMusicVolume(float value)
    {
        musicVolume.Value = value;
        Instance.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume.Value) * 20);
    }

    public void AddSound(Sound sound)
    {
        List<string> allSoundNames = new List<string>();
        for (int i = 0; i < Instance.sounds.Count; i++)
        {
            allSoundNames.Add(Instance.sounds[i].audioName);
        }
        if (!allSoundNames.Contains(sound.audioName)
        && sound.audioClip != null)
        {
            sound.audioSource = Instance.gameObject.AddComponent<AudioSource>();

            sound.audioSource.clip = sound.audioClip;
            if (sound.isSfx)
            {
                sound.audioSource.outputAudioMixerGroup = Instance.sfxMixer;
            }
            else
            {
                sound.audioSource.outputAudioMixerGroup = Instance.musicMixer;
            }
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = sound.playOnAwake;
            
            Instance.sounds.Add(sound);
        }
    }

    public void MassRemoveSound()
    {
        for (int i = Instance.sounds.Count - 1; i >= 0; i--)
        {
            if (!Instance.sounds[i].keep)
            {
                Instance.sounds.RemoveAt(i);
            }
        }
    }
}
