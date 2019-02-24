using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboar : MonoBehaviour
{

    public Camera m_Camera; //main camera

    private void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
