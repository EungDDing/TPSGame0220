using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform player;
    private GameObject obj;
    private PlayerManager playerManager;
    private EndPortal endPortal;
    private Vector3 offset;
    private float rotX;
    private float rotY;
    private bool isAim;

    private bool isEnable;
    private void Awake()
    {
        sensitivity = 400.0f;
        offset = new Vector3(0.5f, 1.5f, -1.5f);
        transform.forward = new Vector3(1.0f, 0.0f, 0.0f);

        isEnable = true;

        if (!player.TryGetComponent<PlayerManager>(out playerManager))
        {
            Debug.Log("CameraMove.cs TryGetComponent<PlayerManager> failed");
        }
        obj = GameObject.Find("EndPortal");
        if (!obj.TryGetComponent<EndPortal>(out endPortal))
        {
            Debug.Log("CameraMove.cs TryGetComponent<EndPortal> failed");
        }
        playerManager.OnAimingChange += AimCamera;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;       
    }

    private void Update()
    {
        ChangeMouseState();
        RotateCamera();
    }
    public void RotateCamera()
    {
        if (!isEnable)
            return;
        if (isAim)
        {
            offset = new Vector3(0.4f, 1.5f, -0.6f);
        }
        else
        {
            offset = new Vector3(0.5f, 1.5f, -1.5f);
        }
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -15.0f, 10.0f);

        transform.rotation = Quaternion.Euler(-rotY, rotX, 0.0f);
        transform.position = transform.rotation * offset + player.position;
    }
    public void AimCamera(bool newAimState)
    {
        isAim = newAimState;
    }
    public void SetEnable(bool state)
    {
        isEnable = state;
    }
    public void ChangeMouseState()
    {
        if (isEnable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
