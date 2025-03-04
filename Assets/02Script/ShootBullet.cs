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

    public Action<bool> OnAimingChange;

    private bool isAiming;

    private void Awake()
    {
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
    }
}
