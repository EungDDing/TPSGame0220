using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] private GameObject shpere;
    private float distance;
    private Vector3 spawnPoint;
    private Ray ray;
    private Vector3 offset;
    private void Awake()
    {
        offset = Vector3.zero;
        distance = 100.0f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateSphere();
            transform.forward = new Vector3(ray.direction.x, 0.0f, ray.direction.z);
        }
    }

    public void CreateSphere()
    {
        offset.x = Random.Range(-0.05f, 0.05f);
        offset.y = Random.Range(-0.05f, 0.05f);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            spawnPoint = hit.point;
        }
        else
        {
            spawnPoint = ray.GetPoint(distance);
        }

        Instantiate(shpere, spawnPoint + offset, Quaternion.identity);
    }
}
