using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponPosition : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (!player.TryGetComponent<Animator>(out animator))
        {
            Debug.Log("SetWeaponPosition() - Awake() failed");
        }

        if (animator.GetBool("IsWalk"))
        {
            transform.position = new Vector3(0.00024f, 0.00075f, -0.00033f);
        }
    }
}
