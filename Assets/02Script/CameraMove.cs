using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform player;
    private ShootBullet shootBullet;
    private Vector3 offset;
    private float rotX;
    private float rotY;

    private void Awake()
    {
        sensitivity = 400.0f;
        offset = new Vector3(0.5f, 1.5f, -1.5f);
        transform.forward = new Vector3(0.0f, 0.0f, 1.0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!player.TryGetComponent<ShootBullet>(out shootBullet))
        {
            Debug.Log("CameraMove.cs TryGetComponent<ShootBullet> failed");
        }

        shootBullet.OnAimingChange += AimCamera;
    }

    private void Update()
    {
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -15.0f, 20.0f);

        transform.rotation = Quaternion.Euler(-rotY, rotX, 0.0f);
        transform.position = player.position + transform.rotation * offset;
    }

    public void AimCamera(bool newAimingState)
    {
        Debug.Log("조준중 offset 조정");
        
    }
}
