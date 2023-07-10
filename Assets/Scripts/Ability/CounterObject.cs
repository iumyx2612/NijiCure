using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animation))]
public class CounterObject : MonoBehaviour
{
    private Animation animation;
    private AnimationClip clip;
    private string animName;


    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    // Any workaround to reduce the number of call?
    private void OnEnable()
    {
        if (clip != null)
        {
            animation.Play(animName);
        }
    }

    public void AddAnim(AnimationClip _clip, string _animName)
    {
        clip = _clip;
        animName = _animName;
        animation.AddClip(_clip, _animName);
    }
}
