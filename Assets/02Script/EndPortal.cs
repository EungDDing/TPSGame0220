using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortal : MonoBehaviour
{
    private CameraMove cameraMove;
    private PlayerManager playerManager;
    public delegate void EnterPortal();
    public event EnterPortal OnEnterPortal;

    private void Awake()
    {
        Camera cam = Camera.main;
        cam.TryGetComponent<CameraMove>(out cameraMove);
        GameObject player = GameObject.FindWithTag("Player");
        player.TryGetComponent<PlayerManager>(out playerManager);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraMove.SetEnable(false);
            playerManager.SetEnable(false);
            OnEnterPortal?.Invoke();
        }
    }
}
