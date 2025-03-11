using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private int animHash_Run = Animator.StringToHash("IsRun");
    private int animHash_Fire = Animator.StringToHash("IsFire");

    private GameObject player;

    private EnemyAI enemyAI;

    private int maxHP = 100;
    public int currentHP;

    private float damage = 3;
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
    private void Awake()
    {
        player = GameObject.Find("Player");
        TryGetComponent<NavMeshAgent>(out agent);
        transform.TryGetComponent<Animator>(out animator);
        TryGetComponent<EnemyAI>(out enemyAI);
        
        if (enemyAI != null)
        {
            enemyAI.StartAI();
        }
    }
    private void Update()
    {
        if (agent != null)
        {
            if (agent.velocity.sqrMagnitude > 0.2f)
            {
                animator.SetBool(animHash_Run, true);
            }
        }
    }
    private void InitEnemyHP()
    {
        currentHP = maxHP;
    }
    public void AttackTarget()
    {
        agent.isStopped = true;
        animator.SetTrigger(animHash_Fire);
    }
    public void InitEnemy()
    {
        agent.speed = 3.0f;
        agent.stoppingDistance = 10.0f;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        InitEnemyHP();
    }
}
