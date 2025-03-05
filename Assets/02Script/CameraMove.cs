using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform player;
    private PlayerManager playerManager;
    private Vector3 offset;
    private float rotX;
    private float rotY;
    private bool isAim;

    private void Awake()
    {
        sensitivity = 400.0f;
        offset = new Vector3(0.5f, 1.5f, -1.5f);
        transform.forward = new Vector3(0.0f, 0.0f, 1.0f);

        if (!player.TryGetComponent<PlayerManager>(out playerManager))
        {
            Debug.Log("CameraMove.cs TryGetComponent<PlayerManager> failed");
        }
        
        playerManager.OnAimingChange += AimCamera;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;       
    }

    private void Update()
    {
        if (isAim) 
        {
            offset = new Vector3(0.5f, 1.5f, -0.7f);
        }
        else
        {
            offset = new Vector3(0.5f, 1.5f, -1.5f);
        }
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -15.0f, 10.0f);

        transform.rotation = Quaternion.Euler(-rotY, rotX, 0.0f);
        transform.position = player.position + transform.rotation * offset;
    }

    public void AimCamera(bool newAimState)
    {
        isAim = newAimState;
    }
}
