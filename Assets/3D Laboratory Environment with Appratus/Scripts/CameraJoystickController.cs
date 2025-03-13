using UnityEngine;
using SimpleInputNamespace; // Ensure this namespace is included

public class CameraJoystickController : MonoBehaviour
{
    public Joystick joystick; // Assign this in the Inspector
    public float rotationSpeed = 10f; // Adjust for sensitivity
    public Transform cameraTransform; // Drag the camera here

    private float rotationX = 0f; // Keep track of vertical rotation

    private void Update()
    {
        if (joystick == null || cameraTransform == null) return;

        // Get joystick input
        float horizontal = joystick.Value.x * rotationSpeed * Time.deltaTime;
        float vertical = joystick.Value.y * rotationSpeed * Time.deltaTime;

        // Rotate camera left/right
        cameraTransform.Rotate(Vector3.up * horizontal, Space.World);

        // Rotate camera up/down (clamped to avoid flipping)
        rotationX -= vertical;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f); // Prevent full rotation

        cameraTransform.localRotation = Quaternion.Euler(rotationX, cameraTransform.localEulerAngles.y, 0);
    }
}
