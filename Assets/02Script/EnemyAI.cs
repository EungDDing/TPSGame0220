using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Enemy_State
{
    Idle,
    Run,
    Attack,
    Die
}
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Enemy enemy;
    private GameObject mainTarget;
    private Enemy_State state;

    private bool isInit;

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        TryGetComponent<Enemy>(out enemy);
        state = Enemy_State.Idle;
        enemy.OnEnemyDie += SetDie;
    }
    public void StartAI()
    {
        isInit = true;
        agent.isStopped = true;
        ChangeState(state);
    }
    public void ChangeState(Enemy_State newState)
    {
        if (isInit)
        {
            StopAllCoroutines();
            state = newState;
            StartCoroutine(state.ToString());
        }
    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator Run()
    {
        agent.isStopped = false;
        while (mainTarget != null)
        {
            if (GetDistanceToTarget() < 15.0f)
            {
                ChangeState(Enemy_State.Attack);
            }
            else
            {
                enemy.StopAttack();
                SetTargetPos(mainTarget.transform.position);
            }
            yield return new WaitForSeconds(1.0f);
        }
        
    }
    IEnumerator Attack()
    {
        agent.isStopped = true;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (mainTarget != null && GetDistanceToTarget() < 15.0f)
            {
                transform.LookAt(mainTarget.transform);
                transform.rotation *= Quaternion.Euler(0.0f, 40.0f, 0.0f);
                enemy.AttackTarget();
            }
            else
            {
                ChangeState(Enemy_State.Run);
            }

        }
    }
    IEnumerator Die()
    {
        if (state == Enemy_State.Die)
        {
            enemy.EnemyIsDead();
            state = Enemy_State.Die;
        }
  
        yield return new WaitForSeconds(1.0f);
    }
    private float GetDistanceToTarget()
    {
        if (mainTarget != null)
            return Vector3.Distance(transform.position, mainTarget.transform.position);
        return float.MaxValue;
    }
    public void SetTarget(GameObject newTarget)
    {
        if (state == Enemy_State.Idle || state == Enemy_State.Run)
        {
            mainTarget = newTarget;
            ChangeState(Enemy_State.Run);
            Debug.Log(mainTarget.tag);
        }
    }
    private void SetTargetPos(Vector3 newTarget)
    {
        if (NavMesh.SamplePosition(newTarget, out NavMeshHit hit, 15.0f, NavMesh.AllAreas))
        {
            newTarget = hit.position;
            agent.SetDestination(newTarget);
        }
    }
    private void SetDie()
    {
        state = Enemy_State.Die;
        ChangeState(state);
    }
}
