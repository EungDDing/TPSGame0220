using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    private Vector3 offset;
    private float rotX;
    private float rotY;

    private void Start()
    {
        sensitivity = 400.0f;
        offset = new Vector3(0.5f, 1.5f, -1.5f);
        transform.rotation = Quaternion.identity;
        cam.transform.position = offset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -30.0f, 60.0f);

        transform.rotation = Quaternion.Euler(-rotY, rotX, 0.0f);
    }
}
