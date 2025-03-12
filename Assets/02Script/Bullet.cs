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
    private GameObject attacker;
    private GameObject obj;
    private Rigidbody rig;
    private SphereCollider col;
    private Enemy enemy;
    private float spanTime;
    private int bulletDamage;
    private void Awake()
    {
        obj = GameObject.Find("AimObject");
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

        spanTime = 3.0f;
    }
    private void Update()
    {
        spanTime -= Time.deltaTime;
        if (spanTime <= 0)
        {
            DestoryBullet();
            
        }
            
        moveBullet();
    }
    public void InitBullet(Vector3 spawnPos, Vector3 targetPos, GameObject owner, int damage, float speed)
    {
        moveDir = targetPos - spawnPos;
        moveDir.Normalize();
        moveSpeed = speed;
        bulletDamage = damage;
        attacker = owner;
        transform.forward = moveDir;
    }
    private void moveBullet()
    {
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
    private void DestoryBullet()
    {
        PoolManager.instance.ReturnToPool(this);
        spanTime = 3.0f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.TryGetComponent<Enemy>(out enemy);
            enemy.TakeDamage(bulletDamage);
        }
        DestoryBullet();
    }
}
