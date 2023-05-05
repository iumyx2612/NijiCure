using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    public Transform centerPoint;
    
    public Rigidbody2D rb;

    public float orbitSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion q = Quaternion.AngleAxis (orbitSpeed * Time.fixedDeltaTime, transform.forward);
        rb.MovePosition (q * (rb.transform.position - centerPoint.position) + centerPoint.position);
        rb.MoveRotation (rb.transform.rotation * q);
    }
}
