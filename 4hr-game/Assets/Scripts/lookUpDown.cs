using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookUpDown : MonoBehaviour
{


    private void Update()
    {

        float mouseInput = Input.GetAxis("Mouse Y");
        Vector3 lookup = new Vector3(-mouseInput, 0, 0);
        transform.Rotate(lookup);

    }



}
