using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    // kind of prefabs
    [SerializeField] private GameObject prefabs;
    private int poolSize = 5;
    // the array of object that in object pool
    private Queue<Bullet> bulletPool;
    private Bullet bullet;
    private GameObject obj;
    // objectPool[0] => bullet object pool
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bulletPool = new Queue<Bullet>();

        Allocate();
    }
    private void Allocate()
    {
        for (int i = 0; i < poolSize; i++)
        {
            obj = Instantiate(prefabs);
            if (obj.TryGetComponent<Bullet>(out bullet))
            {
                bulletPool.Enqueue(bullet);
            }
            obj.SetActive(false);
        }
    }
    private Bullet ActiveBullet()
    {
        if (bulletPool.Count < 1)
        {
            Allocate();
        }
        return bulletPool.Dequeue();
    }
    public void ReturnToPool(Bullet returnBullet)
    {
        returnBullet.gameObject.SetActive(false);
        bulletPool.Enqueue(returnBullet);
    }
    public void FireBullet(Vector3 spawnPos, Vector3 targetPos, GameObject owner, int damage, float speed)
    {
        bullet = ActiveBullet();
        
        if (bullet != null)
        {
            bullet.transform.position = spawnPos;
            bullet.gameObject.SetActive(true);
            bullet.InitBullet(spawnPos, targetPos, owner, damage, speed);
        }
    }
}
