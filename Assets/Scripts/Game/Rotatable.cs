using System;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    private float sensibility;

    [SerializeField] private float maxRotation;
    
    private Vector2 lastMousePosition;

    private Rigidbody rb;
    private float angle;

    private bool isMouseDown;

    private Camera cam,
        uiCam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        sensibility = GameManager.instance.config.RotationSensibility;

        cam = Camera.main;
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
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            
            float xMovement = 0,
                yMovement = 0;

            Vector2 screenPoint = cam.WorldToScreenPoint(transform.position);
            
            if (x > screenPoint.x)
            {
                yMovement = (Input.mousePosition.y - lastMousePosition.y) * sensibility;
            }
            else
            {
                yMovement = -(Input.mousePosition.y - lastMousePosition.y) * sensibility;
            }

            if (y > screenPoint.y)
            {
                xMovement = -(Input.mousePosition.x - lastMousePosition.x) * sensibility;
            }
            else
            {
                xMovement = (Input.mousePosition.x - lastMousePosition.x) * sensibility;
            }

            float movement = xMovement + yMovement;
            movement = Mathf.Clamp(movement, -maxRotation * Time.deltaTime, 
                maxRotation * Time.deltaTime);
            angle += movement;

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
