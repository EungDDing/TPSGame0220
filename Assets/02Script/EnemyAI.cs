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
    }
    public void StartAI()
    {
        isInit = true;
        state = Enemy_State.Idle;
        mainTarget = null;
        Debug.Log(agent);
        Debug.Log(enemy);
        agent.isStopped = false;

        ChangeState(state);
    }
    public void ChangeState(Enemy_State newState)
    {
        if (isInit)
        {
            StopCoroutine(newState.ToString());
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
        while (true)
        {
            if (GetDistanceToTarget() < 10.0f)
            {
                ChangeState(Enemy_State.Attack);
            }
            else
            {
                SetTargetPos(mainTarget.transform.position);
            }
            yield return new WaitForSeconds(1.0f);
        }
        
    }
    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (GetDistanceToTarget() < 10.0f)
            {
                transform.LookAt(mainTarget.transform);
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
        yield return new WaitForSeconds(1.0f);
    }
    private float GetDistanceToTarget()
    {
        if (mainTarget != null)
            return Vector3.Distance(transform.position, mainTarget.transform.position);
        return -1;
    }
    public void SetTarget(GameObject newTarget)
    {
        if (state == Enemy_State.Idle)
        {
            mainTarget = newTarget;
            ChangeState(Enemy_State.Run);
            Debug.Log(mainTarget.tag);
        }
    }
    private void SetTargetPos(Vector3 newTarget)
    {
        if (NavMesh.SamplePosition(newTarget, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
        {
            newTarget = hit.position;
            agent.SetDestination(newTarget);
        }
    }
}
