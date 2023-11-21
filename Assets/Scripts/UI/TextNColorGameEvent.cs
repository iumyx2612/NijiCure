using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TextNColor
{
    public string text;
    public Color color;

    public TextNColor(string _text, Color _color)
    {
        text = _text;
        color = _color;
    }
}


namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Game Event/Text and Color")]
    public class TextNColorGameEvent : GameEventBase<TextNColor>
    {
        
    }
}

