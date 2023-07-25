using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class CounterObject : MonoBehaviour
{
    [SerializeField] private string animName;
    private Animator animator;
    private float animLength;
    private float internalLength;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.Play(animName);
        animLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private void Update()
    {
        internalLength += Time.deltaTime;
        if (internalLength >= animLength)
        {
            gameObject.SetActive(false);
            internalLength = 0f;
        }
    }
}
