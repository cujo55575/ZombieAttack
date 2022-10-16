using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private float yaw;
    private float pitch;
    private float lookSpeedH = 2f;
    private float lookSpeedV = 2f;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(0f, yaw, 1f);

        }
    }
}
