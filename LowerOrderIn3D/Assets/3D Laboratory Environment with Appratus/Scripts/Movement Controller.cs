using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform cameraTransform;
    public CharacterController characterController;

    [Header("Settings")]
    public float cameraSensitivity;
    public float moveSpeed;
    public float moveInputDeadZone;

    [Header("Touch Detection")]
    int leftFingerId, rightFingerId;
    float halfScreenWidth;

    [Header("Camera Control")]
    Vector2 lookInput;
    float cameraPitch;

    [Header("Player Movement")]
    Vector2 moveTouchStartPosition;
    Vector2 moveInput;

    [Header("Gravity & Jumping")]
    public float stickToGroundForce = 10;
    public float gravity = 10;
    public float jumpForce = 10;
    private float verticalVelocity;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayers;
    public float groundCheckRadius;
    private bool grounded;

    [Header("Touch Interaction")]
    public LayerMask interactableLayer;
    public float raycastDistance = 2f; 

    private void Start()
    {
        cameraTransform.eulerAngles = new Vector3(-5f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);

        leftFingerId = -1;
        rightFingerId = -1;

        halfScreenWidth = Screen.width / 2;

        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);
    }

    private void Update()
    {
        GetTouchInput();

        if (rightFingerId != -1)
        {
            LookAround();
        }

        if (leftFingerId != -1)
        {
            Move();
        }

        MoveY();
    }

    private void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers);
    }

    private void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    if (touch.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                        leftFingerId = touch.fingerId;
                        //Debug.Log("Tracking Left");

                        moveTouchStartPosition = touch.position;
                    }
                    else if (touch.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        rightFingerId = touch.fingerId;
                        //Debug.Log("Tracking Right");
                    }

                    break;

                case TouchPhase.Ended:

                case TouchPhase.Canceled:

                    if (touch.fingerId == leftFingerId)
                    {
                        leftFingerId = -1;
                        //Debug.Log("Stopped tracking left");
                    }
                    else if (touch.fingerId == rightFingerId)
                    {
                        rightFingerId = -1;
                        //Debug.Log("Stopped tracking right");
                    }

                    break;

                case TouchPhase.Moved:

                    if (touch.fingerId == rightFingerId)
                    {
                        lookInput = touch.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    else if (touch.fingerId == leftFingerId)
                    {
                        moveInput = touch.position - moveTouchStartPosition;
                    }

                    break;

                case TouchPhase.Stationary:

                    if (touch.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }

                    break;
            }
        }
    }

    public bool IsTap(Touch touch)
    {
        float tapThreshold = 50f;
        float tapDuration = 0.2f;

        return touch.phase == TouchPhase.Ended &&
               touch.deltaPosition.magnitude > tapThreshold &&
               touch.deltaTime < tapDuration;
    }

    public void Jump()
    {
        if (grounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void LookAround()
    {
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        transform.Rotate(transform.up, lookInput.x);
    }

    private void Move()
    {
        if (moveInput.sqrMagnitude <= moveInputDeadZone) return;
        Vector2 movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;
        characterController.Move(transform.right * movementDirection.x + transform.forward * movementDirection.y);
    }

    private void MoveY()
    {
        if (grounded && verticalVelocity <= 0)
        {
            verticalVelocity = -stickToGroundForce * Time.deltaTime;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        Vector3 verticalMovement = transform.up * verticalVelocity;
        characterController.Move(verticalMovement * Time.deltaTime);
    }
}