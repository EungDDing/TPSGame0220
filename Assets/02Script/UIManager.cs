using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image aimImage;
    [SerializeField] private Image normalImage;
    [SerializeField] private TextMeshProUGUI bulletCount;
    [SerializeField] private TextMeshProUGUI hasBulletCount;
    [SerializeField] private Image missionClear;
    [SerializeField] private TextMeshProUGUI hpText;
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
        playerManager.OnHPChange += HPChange;
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
        {
            normalImage.gameObject.SetActive(false);
            aimImage.gameObject.SetActive(true);
        }
        else
        {
            normalImage.gameObject.SetActive(true);
            aimImage.gameObject.SetActive(false);
        }
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
    private void HPChange(int currentHP)
    {
        hpText.text = currentHP.ToString();
    }
}
