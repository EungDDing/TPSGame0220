using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MissionClear : MonoBehaviour
{
    [SerializeField] private Button okButton;
    private CameraMove cameraMove;
    private PlayerManager playerManager;
    private void Awake()
    {
        Camera cam = Camera.main;
        cam.TryGetComponent<CameraMove>(out cameraMove);
        GameObject player = GameObject.FindWithTag("Player");
        player.TryGetComponent<PlayerManager>(out playerManager);

        okButton.onClick.AddListener(OnClickOkButton);
    }
    public void OnClickOkButton()
    {
        okButton.transform.parent.gameObject.SetActive(false);
        cameraMove.SetEnable(true);
        playerManager.SetEnable(true);
    }
}
