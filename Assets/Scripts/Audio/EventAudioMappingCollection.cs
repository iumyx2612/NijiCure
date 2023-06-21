using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Event-Audio mapping collection")]
public class EventAudioMappingCollection : ScriptableObject
{
    public List<EventAudioMapping> mappingList = new();
}
