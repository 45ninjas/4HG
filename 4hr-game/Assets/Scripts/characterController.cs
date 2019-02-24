using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    float acceleration = 2.5f;
    [SerializeField]
    float deAcceleration = 5;

    [SerializeField,FormerlySerializedAs("JumpVelocity")]
    // How fast the player jumps.
    float JumpSpeed = 15;
    [SerializeField]
    // How quickly the upwards speed of the jump drops down to zero.
    float JumpDecay = 20;

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

    // This is hidden from the inspector but other scripts will need to access it.
    // This is the direction a player is looking.
    [HideInInspector]
    public Vector3 LookNormal;

    public bool IsCrouched = false;
    public bool IsGrounded = false;
    public bool IsRunning = false;
    public bool IsJumping = false;

    // Keep track of the jump velocity.
    private float jumpVelocity = 0;

    // The gravity of the player controller.
    public float Gravity = -13;

    // the speed the player is currently moving at.
    private float moveSpeed;

    private void Start()
    {
        // Get the character controller and the current height.
        controller = GetComponent<CharacterController>();
        standardHeight = camera.localPosition.y;
    }

    private void Update()
    {
        // Set the running and grounded booleans.
        IsRunning = Input.GetButton("Run");
        IsGrounded = controller.isGrounded;

        // Do the crouch code.
        DoCrouch();

        // Get the direction the player want's to move.
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // This is used a-lot.
        float accel = acceleration * Time.deltaTime;

        // Slow the player down if there is no input.
        if (direction == Vector3.zero && moveSpeed > 0)
            moveSpeed -= accel;
        // There is input, so speed the player up.
        else
            moveSpeed += accel;

        // Set the speed limit for the function the player is currently doing.
        float speedLimit = Speed;
        if (IsRunning)
            speedLimit = RunSpeed;
        if (IsCrouched)
            speedLimit = CrouchSpeed;

        // Enforce the speed limit.
        if (moveSpeed > speedLimit)
        {
            // Get the amount the player is over speeding.
            float delta = moveSpeed - speedLimit;

            if (delta >= deAcceleration * Time.deltaTime)
               moveSpeed -= deAcceleration * Time.deltaTime;
            else
                moveSpeed -= delta;
        }


        // Player can only jump if, they are on the ground, not jumping and not crouching..
        if (Input.GetButtonDown("Jump") && IsGrounded && !IsJumping && !IsCrouched)
        {
            jumpVelocity = JumpSpeed;
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
        IsCrouched = Input.GetButton("Duck");

        // TODO: Make sure the player can stand back up again.
        // TODO: Make the player crouch if something heavy is on-top of them?

        if (Input.GetButtonDown("Duck"))
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