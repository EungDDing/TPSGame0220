using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCameraMove : MonoBehaviour
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
        offset = new Vector3(0.4f, 1.5f, -0.6f);
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
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);




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
}
