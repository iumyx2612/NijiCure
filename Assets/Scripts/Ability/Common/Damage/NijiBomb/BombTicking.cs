using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTicking : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    public void Explode() // Used in Anim Event
    {
        gameObject.SetActive(false);
        explosion.SetActive(true);
    }
}
