using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHP = 100;
    public int currentHP;
    
    void Start()
    {
        InitEnemyHP();    
    }

    private void InitEnemyHP()
    {
        currentHP = maxHP;
    }
}
