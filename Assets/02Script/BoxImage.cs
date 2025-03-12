using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxImage : MonoBehaviour
{
    [SerializeField] private Image Image;
    private Transform player;
    private Vector3 faceDir;

    private void Awake()
    {
        GameObject obj;
        obj = GameObject.FindWithTag("Player");
        player = obj.transform;
    }
    private void Update()
    {
        faceDir = transform.position - player.position;
        faceDir.y = 0.0f;
        transform.forward = faceDir;
    }
}

