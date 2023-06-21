using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private EventAudioMappingCollection eventAudioMappingCollection;

    private Dictionary<EventAudioMapping, Action> eventActions = new();

    private void OnEnable()
    {
        if (eventAudioMappingCollection != null)
        {
            foreach (var eventAudioMapping in eventAudioMappingCollection.mappingList)
            {
                eventAudioMapping.audioSource = gameObject.AddComponent<AudioSource>();
                var action = new Action(() =>
                {
                    eventAudioMapping.PlayAudio();
                });
                eventActions.Add(eventAudioMapping, action);
                eventAudioMapping.gameEventBaseObject.AddListener(action);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var eventAction in eventActions)
        {
            eventAction.Key.gameEventBaseObject.RemoveListener(eventAction.Value);
        }
    }
}
