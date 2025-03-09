using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public CharacterController characterController;
    public SimpleInputNamespace.Joystick movementJoystick;
    public Transform playerCamera;
    public float moveSpeed = 5f;

    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
    }

    // void Update()
    // {
    //     // Get joystick input from SimpleInput
    //     float moveX = SimpleInput.GetAxis("Horizontal"); // Joystick X-axis
    //     float moveZ = SimpleInput.GetAxis("Vertical");   // Joystick Y-axis

    //     // Convert input to movement direction
    //     Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

    //     // Move the player
    //     if (moveDirection.magnitude > 0.1f)
    //     {
    //         characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    //     }
    // }

     void Update()
    {
        if (movementJoystick == null || playerCamera == null)
        {
            Debug.LogWarning("Joystick or Camera is not assigned!");
            return;
        }

        // Get input from the specific joystick
        float moveX = movementJoystick.xAxis.value; // X-axis input
        float moveZ = movementJoystick.yAxis.value; // Y-axis input

        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        camForward.y = 0; // Ignore vertical tilt
        camRight.y = 0;   // Ignore vertical tilt

        camForward.Normalize();
        camRight.Normalize();

        // Convert input to movement direction relative to camera
        Vector3 moveDirection = (camForward * moveZ + camRight * moveX).normalized;

        // Move the player
        if (moveDirection.magnitude > 0.1f)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}



