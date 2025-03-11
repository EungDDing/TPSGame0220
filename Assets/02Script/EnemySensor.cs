using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class EnemySensor : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider col;
    private EnemyAI enemyAI;
    private void Awake()
    {
        if (TryGetComponent<Rigidbody>(out rb))
        {
            rb.useGravity = false;
        }

        if (TryGetComponent<SphereCollider>(out col))
        {
            col.isTrigger = true;
            col.radius = 15.0f;
        }

        if (!TryGetComponent<EnemyAI>(out enemyAI))
        {
            Debug.Log("EnemySensor.cs TryGetComponent<EnemyAI> failed");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.SetTarget(other.gameObject);
        }

    }
}
