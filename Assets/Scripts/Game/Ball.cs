using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        velocity = Mathf.Min(velocity.magnitude, 10) * velocity.normalized;
        rb.velocity = velocity;
    }

    public void SetMaterial(Material material)
    {
        GetComponentInChildren<MeshRenderer>().sharedMaterial = material;
    }
}
