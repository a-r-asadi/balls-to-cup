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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        sensibility = GameManager.instance.config.RotationSensibility;
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
            
            if (x > Screen.width * 0.5f)
            {
                yMovement = (Input.mousePosition.y - lastMousePosition.y) * sensibility;
            }
            else
            {
                yMovement = -(Input.mousePosition.y - lastMousePosition.y) * sensibility;
            }

            if (y > Screen.height * 0.5f)
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
