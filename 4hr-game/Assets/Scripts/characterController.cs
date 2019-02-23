using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class characterController : MonoBehaviour
{


    public float speed = 1.0F;
    public float rotateSpeed = 1.0F;

    private void Update()
    {



        CharacterController controller = GetComponent<CharacterController>();

        float mouseInput = Input.GetAxis("Mouse X");
        Vector3 lookhere = new Vector3(0, mouseInput, 0);
        transform.Rotate(lookhere);

        Vector3 forward = transform.forward;
        float curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);

        Vector3 sideways = new Vector3(0, 5, 0);
        float xurSpeed = speed * Input.GetAxis("Horizontal");
        controller.SimpleMove(sideways * xurSpeed);




    }
}