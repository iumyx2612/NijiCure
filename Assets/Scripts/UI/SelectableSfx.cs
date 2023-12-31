using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableSfx : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.Play("Navigate");
    }
}
