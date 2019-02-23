using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class characterController : MonoBehaviour
{


    public float speed = 1.0F;
    public float rotateSpeed = 1.0F;


    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Verticle");
        controller.SimpleMove(forward * curSpeed);
    }
}
