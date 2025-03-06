using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 moveDir;
    private Transform targetPos;
    private GameObject obj;
    private Rigidbody rig;
    private SphereCollider col;
    private void Awake()
    {
        obj = GameObject.Find("AimObject");
        Debug.Log(obj.transform.position);
        obj.TryGetComponent<Transform>(out targetPos);

        if (TryGetComponent<Rigidbody>(out rig))
        {
            rig.useGravity = false;
        }
        if (TryGetComponent<SphereCollider>(out col))
        {
            col.isTrigger = true;
            col.radius = 0.01f;
        }

        moveSpeed = 1.0f;
        moveDir = targetPos.position - transform.position;
        transform.forward = moveDir;
    }
    private void Update()
    {
        moveBullet();
    }
    private void moveBullet()
    {
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
}
