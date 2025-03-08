using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image aimImage;
    [SerializeField] private TextMeshProUGUI bulletCount;
    [SerializeField] private TextMeshProUGUI hasBulletCount;
    private GameObject obj;
    private PlayerManager playerManager;
    private bool isAim;
    private void Awake()
    {
        obj = GameObject.FindWithTag("Player");
        if (!obj.TryGetComponent<PlayerManager>(out playerManager))
        {
            Debug.Log("UIManager.cs TryGetComponent<PlayerManager> fail");
        }
        playerManager.OnAimingChange += SetAimState;
        playerManager.OnCurBulletCountChange += ChangeCurBulletCount;
        playerManager.OnHasBulletCountChange += ChangeHasBulletCount;
    }
    void Update()
    {
        ChangeAimImageActive();
    }
    private void SetAimState(bool newAimState)
    {
        isAim = newAimState;
    }
    private void ChangeAimImageActive()
    {
        if (isAim)
            aimImage.gameObject.SetActive(true);
        else
            aimImage.gameObject.SetActive(false);
    }
    private void ChangeCurBulletCount(int count)
    {
        bulletCount.text = count.ToString();
    }
    private void ChangeHasBulletCount(int count)
    {
        hasBulletCount.text = "/ " + count.ToString();
    }

}
