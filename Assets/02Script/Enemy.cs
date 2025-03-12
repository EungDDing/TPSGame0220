using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform firePos;
    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 shootOffset = Vector3.zero;
    private int animHash_Run = Animator.StringToHash("IsRun");
    private int animHash_Fire = Animator.StringToHash("IsFire");
    private int animHash_Die = Animator.StringToHash("IsDie");

    private GameObject playerObject;
    private PlayerManager player;

    private EnemyAI enemyAI;

    private int maxHP = 100;
    public int currentHP;
    private bool isDie;
    private int damage = 3;
    public delegate void OnChangeHP(int hp);
    public event OnChangeHP ChangeHP;
    public delegate void EnemyDie();
    public event EnemyDie OnEnemyDie;

    RaycastHit hit;
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = value;
            if (currentHP <= 0)
                currentHP = 0;
            Debug.Log(currentHP);
            ChangeHP?.Invoke(currentHP);
        }
    }
    private void Awake()
    {
        isDie = false;
        playerObject = GameObject.Find("Player");
        playerObject.TryGetComponent<PlayerManager>(out player);
        TryGetComponent<NavMeshAgent>(out agent);
        transform.TryGetComponent<Animator>(out animator);
        TryGetComponent<EnemyAI>(out enemyAI);
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

        shootOffset.x = Random.Range(-0.3f, 0.3f);
        shootOffset.y = Random.Range(-0.3f, 0.3f);
        shootOffset.z = 0.0f;

        Vector3 ray = (playerObject.transform.GetChild(1).position + shootOffset) - firePos.position;

        Debug.DrawLine(firePos.position, playerObject.transform.GetChild(1).position + shootOffset, Color.red, 0.5f);
        if (Physics.Raycast(firePos.position, ray, out hit, 15.0f))
        {
            Debug.Log("Fire");
            
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit");
                player.TakeDamage(damage);
            }
            else if (hit.collider.CompareTag("Architecture") || hit.collider.CompareTag("Cover"))
            {
                Debug.Log("Hit Cover");
            }
        }
    }
    public void StopAttack()
    {
    }
    public void InitEnemy()
    {
        agent.speed = 3.0f;
        agent.stoppingDistance = 15.0f;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        InitEnemyHP();

        enemyAI.StartAI();
    }
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP == 0 && !isDie)
        {
            isDie = true;
            OnEnemyDie?.Invoke();
        }
    }
    public void EnemyIsDead()
    {
        agent.isStopped = true;
        animator.SetTrigger(animHash_Die);
        float deathTime = 2.3f;
        StartCoroutine(DestroyEnemy(deathTime));
    }
    IEnumerator DestroyEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        animator.enabled = false;
        Destroy(gameObject, 2.0f);
    }
}
