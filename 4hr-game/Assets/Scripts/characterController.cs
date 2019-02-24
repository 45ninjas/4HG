using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class characterController : MonoBehaviour
{
    // Movement speeds.
    [SerializeField]
    float Speed = 2.5f;
    [SerializeField]
    float RunSpeed = 4.5f;
    [SerializeField]
    float CrouchSpeed = 1.2f;
    // How fast the player accelerates towards the target velocity.
    [SerializeField]
    float Acceleration = 2.5f;

    [SerializeField]
    float JumpVelocity = 15;
    [SerializeField]
    float JumpRampSpeed = 15;

    public float JumpDecay = 20;

    // Mouse sensitivity.
    [Range(0.5f, 10f)]
    public float Sensitvity = 2.5f;

    // How high and low to look up and down.
    [SerializeField]
    float VerticalLookLimit = 86;

    // How high the player is when crouching.
    public float CrouchHehight = 1f;


    private float standardHeight;
    CharacterController controller;

    [SerializeField]
    private Transform camera;

    private Vector2 lookRotaion;

    // This is hidden from the inspector but other scripts need to access it.
    [HideInInspector]
    public Vector3 LookNormal;
    // This is the direction a player is looking.

    public bool IsCrouched = false;
    public bool IsGrounded = false;
    public bool IsRunning = false;
    public bool IsJumping = false;

    private float jumpVelocity = 0;
    private float jumpTimer = 0;

    public float Gravity = -13;

    private float moveSpeed;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        standardHeight = camera.localPosition.y;
    }

    private void Update()
    {
        IsCrouched = Input.GetButton("Duck");
        IsRunning = Input.GetButton("Run");

        IsGrounded = controller.isGrounded;

        DoCrouch();

        // Get the direction the player want's to move.
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float accel = Acceleration * Time.deltaTime;

        if (direction == Vector3.zero && moveSpeed > 0)
            moveSpeed -= accel;
        else
            moveSpeed += accel;

        float targetSpeed = Speed;
        if (IsRunning)
            targetSpeed = RunSpeed;
        if (IsCrouched)
            targetSpeed = CrouchSpeed;

        if (moveSpeed > targetSpeed)
        {
            float delta = moveSpeed - targetSpeed;

            if (delta >= accel * 2)
               moveSpeed -= accel * 2;
            else
                moveSpeed -= delta;
        }


        // Player can only jump if, they are on the ground, not jumping and not crouching..
        if (Input.GetButtonDown("Jump") && IsGrounded && !IsJumping && !IsCrouched)
        {
            jumpVelocity = JumpVelocity;
            IsJumping = true;
        }

        // Decay the jump velocity over time to make the jump feel smoother.
        if (IsJumping)
        {
            jumpVelocity -= Time.deltaTime * JumpDecay;
            if(jumpVelocity < 0)
                IsJumping = false;
        }

        // Move the player
        MovePlayer(direction, moveSpeed, jumpVelocity);
    }

    private void LateUpdate()
    {
        // Rotate the camera
        RotateCamera(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    void RotateCamera(float horizontal, float vertical)
    {
        lookRotaion.x += horizontal * Sensitvity;
        lookRotaion.y -= vertical * Sensitvity;

        lookRotaion.y = Mathf.Clamp(lookRotaion.y, -VerticalLookLimit, VerticalLookLimit);

        transform.localRotation = Quaternion.Euler(0, lookRotaion.x, 0);
        camera.localRotation = Quaternion.Euler(lookRotaion.y, 0, 0);

        LookNormal = camera.forward;
    }

    void MovePlayer(Vector3 direction, float speed, float verticalVelocity)
    {
        // Convert the direction from local space to world space.
        direction = transform.TransformDirection(direction);

        // Keep the magnitude below 1.
        if (direction.magnitude > 1)
            direction.Normalize();

        // Lastly, move the controller in the direction at the move speed being frame rate agnostic.
        Vector3 moveVector = direction * speed;
        //return controller.SimpleMove(moveVector);

        moveVector += Vector3.up * verticalVelocity;
        moveVector += Vector3.up * Gravity;

        controller.Move(moveVector * Time.deltaTime);
    }

    void DoCrouch()
    {
        if(Input.GetButtonDown("Duck"))
        {
            // The player is trying to crouch.
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, CrouchHehight, camera.transform.localPosition.z);
        }
        if (Input.GetButtonUp("Duck"))
        {
            // The player is trying to stand back up.
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, standardHeight, camera.transform.localPosition.z);
        }
    }
}