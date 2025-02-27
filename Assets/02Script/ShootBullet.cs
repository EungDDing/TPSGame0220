using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] private GameObject shpere;
    private float distance;
    private Vector3 spawnPoint;
    private Ray ray;
    private Vector3 offset;

    public Action<bool> OnAimingChange;

    private bool isAiming;

    private void Awake()
    {
        offset = Vector3.zero;
        distance = 100.0f;
    }

    private void Update()
    {
        bool newAimingState = Input.GetMouseButton(1);

        if (newAimingState != isAiming)
        {
            isAiming = newAimingState;
            OnAimingChange?.Invoke(isAiming);
        } 

        if (isAiming && Input.GetMouseButtonDown(0))
        {
            CreateSphere();
        }
    }

    public void CreateSphere()
    {
        offset.x = UnityEngine.Random.Range(-0.05f, 0.05f);
        offset.y = UnityEngine.Random.Range(-0.05f, 0.05f);

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
