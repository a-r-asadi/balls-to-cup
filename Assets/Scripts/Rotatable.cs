using System;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    private Vector2 lastMousePosition;

    private Rigidbody rb;
    private float angle;

    private bool isMouseDown;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isMouseDown = true;
        }
        else if (Input.GetMouseButton(0))
        {
            float x = (Input.mousePosition.x - lastMousePosition.x) * 0.25f;
            x = Mathf.Clamp(x, -5, 5);
            angle += x;

            lastMousePosition = Input.mousePosition;
        }
        else
        {
            isMouseDown = false;
        }
    }

    private void FixedUpdate()
    {
        if (isMouseDown)
        {
            rb.MoveRotation(Quaternion.Euler(0, 0, angle));
        }
    }
}
