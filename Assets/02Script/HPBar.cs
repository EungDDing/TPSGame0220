using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    private Enemy enemy;
    private float maxHP = 100;
    private void Awake()
    {
        GameObject obj;
        obj = GameObject.FindWithTag("Enemy");
        obj.TryGetComponent<Enemy>(out enemy);

        enemy.ChangeHP += FillAmountBar;
    }
    private void FillAmountBar(int hp)
    {
        hpBar.fillAmount = hp / maxHP;
    }
}
