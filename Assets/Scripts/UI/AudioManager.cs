using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.Audio;


[Serializable]
public struct AudioVolume
{
    public float sfxVolume;
    public float musicVolume;

    public AudioVolume(float _sfxVolume, float _musicVolume)
    {
        sfxVolume = _sfxVolume;
        musicVolume = _musicVolume;
    }
}


public class AudioManager : MonoBehaviour
{
    // Manage the saves
    private JsonSerializer jsonSerializer = new JsonSerializer();
    [SerializeField] private List<Sound> sounds;
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

        // First time initialization
        for (int i = 0; i < Instance.sounds.Count; i++)
        {
            Sound sound = Instance.sounds[i];
            if (sound.audioSource == null)
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
            }
        }
        
        try
        {
            AudioVolume audioVolume = jsonSerializer.LoadData<AudioVolume>("/audio-volume.json");
            musicVolume.Value = audioVolume.musicVolume;
            sfxVolume.Value = audioVolume.sfxVolume;
        }
        catch(Exception e)
        {
            Debug.LogError($"{e.Message} {e.StackTrace}");
            musicVolume.Value = 1f;
            sfxVolume.Value = 1f;
        }

        SetMusicVolume(musicVolume.Value);
        SetSfxVolume(sfxVolume.Value);
    }

    private void Start()
    {
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
        if (jsonSerializer.SaveData("/audio-volume.json", new AudioVolume(sfxVolume.Value, musicVolume.Value)))
        {
            Debug.Log("Save Audio Complete!");
        }
        else
        {
            Debug.LogError("Can't save Audio");
        }
    }

    public void SetMusicVolume(float value)
    {
        musicVolume.Value = value;
        Instance.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume.Value) * 20);
        if (jsonSerializer.SaveData("/audio-volume.json", new AudioVolume(sfxVolume.Value, musicVolume.Value)))
        {
            Debug.Log("Save Audio Complete!");
        }
        else
        {
            Debug.LogError("Can't save Audio");
        }
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
