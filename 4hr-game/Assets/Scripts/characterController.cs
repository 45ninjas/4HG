using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class characterController : MonoBehaviour
{
    public float Speed = 1.0f;
    public float Sensitvity = 2.5f;

    public float VerticalLookLimit = 86;

    CharacterController controller;

    [SerializeField]
    private Transform camera;

    private Vector2 lookRotaion;

    [HideInInspector]
    public Vector3 LookNormal;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        // Get the direction the player want's to move.
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Move the player
        MovePlayer(direction);

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

    void MovePlayer(Vector3 direction)
    {
        // Convert the direction from local space to world space.
        direction = transform.TransformDirection(direction);

        // Keep the magnitude below 1.
        if (direction.magnitude > 1)
            direction.Normalize();

        // Now that we have a direction the player moves in, let's move the controller.

        // Set the standard move speed.
        float moveSpeed = Speed;

        // If the player is running, set the move speed to whatever.
        // If the player is crouching, set the move speed here.

        // Lastly, move the controller in the direction at the move speed being frame rate agnostic.
        Vector3 moveVector = direction * moveSpeed /** Time.deltaTime*/;
        controller.SimpleMove(moveVector);
    }
}