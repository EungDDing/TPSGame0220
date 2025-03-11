using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHP = 100;
    public int currentHP;

    public delegate void OnChangeHP(int hp);
    public event OnChangeHP ChangeHP;
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = value;
            Debug.Log(currentHP);
            ChangeHP?.Invoke(currentHP);
        }
    }
    void Start()
    {
        InitEnemyHP();    
    }

    private void InitEnemyHP()
    {
        currentHP = maxHP;
    }
}
