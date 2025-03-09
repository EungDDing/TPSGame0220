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
    [SerializeField] private Image missionClear;
    private GameObject obj;
    private PlayerManager playerManager;
    private EndPortal endPortal;

    private bool isAim;
    private void Awake()
    {
        obj = GameObject.FindWithTag("Player");
        if (!obj.TryGetComponent<PlayerManager>(out playerManager))
        {
            Debug.Log("UIManager.cs TryGetComponent<PlayerManager> fail");
        }
        obj = GameObject.Find("EndPortal");
        if (!obj.TryGetComponent<EndPortal>(out endPortal))
        {
            Debug.Log("UIManager.cs TryGetComponent<EndPortal> fail");
        }
        playerManager.OnAimingChange += SetAimState;
        playerManager.OnCurBulletCountChange += ChangeCurBulletCount;
        playerManager.OnHasBulletCountChange += ChangeHasBulletCount;

        endPortal.OnEnterPortal += ActiveMissionClear;
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
    private void ActiveMissionClear()
    {
        missionClear.gameObject.SetActive(true);
    }
}
