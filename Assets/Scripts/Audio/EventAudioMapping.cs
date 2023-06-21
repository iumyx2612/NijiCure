using ScriptableObjectArchitecture;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Event-Audio mapping")]
public class EventAudioMapping : ScriptableObject
{
    public GameEventBase gameEventBaseObject;
    public AudioClipVariable audioClipVariableObject;

    [NonSerialized]
    public AudioSource audioSource;

    public void PlayAudio()
    {
        if (audioSource != null && audioClipVariableObject.Value != null)
        {
            audioSource.clip = audioClipVariableObject.Value;
            audioSource.Play();
        }
    }
}
