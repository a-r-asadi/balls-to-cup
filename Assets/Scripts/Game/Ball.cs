using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float maxVelocity;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        maxVelocity = GameManager.instance.config.BallMaxVelocity;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        velocity = Mathf.Min(velocity.magnitude, maxVelocity) * velocity.normalized;
        rb.velocity = velocity;
    }

    public void SetMaterial(Material material)
    {
        GetComponentInChildren<MeshRenderer>().sharedMaterial = material;
    }
}
