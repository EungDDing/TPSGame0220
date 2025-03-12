using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    private Transform player;
    private Vector3 faceDir;
    private Enemy enemy;
    private float maxHP = 100;
    private void Awake()
    {
        GameObject obj;
        obj = GameObject.FindWithTag("Player");
        enemy = GetComponentInParent<Enemy>();
        
        player = obj.transform;
    }
    private void OnEnable()
    {
        enemy.ChangeHP += FillAmountBar;
        enemy.OnEnemyDie += SetDisable;
    }
    private void OnDisable()
    {
        enemy.ChangeHP -= FillAmountBar;
        enemy.OnEnemyDie -= SetDisable;
    }
    private void Update()
    {
        faceDir = transform.position - player.position;
        faceDir.y = 0.0f;
        transform.forward = faceDir;
    }
    private void FillAmountBar(int hp)
    {
        hpBar.fillAmount = hp / maxHP;
    }
    private void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
