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

        transform.localPosition = new Vector3(0.000148f, 0.000739f, -0.000441f);
        transform.localRotation = Quaternion.Euler(new Vector3(17.976f, 37.402f, -61.164f));
    }
    private void Update()
    {
        if (animator.GetBool("IsWalk"))
        {
            transform.localPosition = new Vector3(0.00024f, 0.00075f, -0.00033f);
            transform.localRotation = Quaternion.Euler(new Vector3(3.762f, 27.961f, -61.571f));
        }
        else
        {
            transform.localPosition = new Vector3(0.000148f, 0.000739f, -0.000441f);
            transform.localRotation = Quaternion.Euler(new Vector3(17.976f, 37.402f, -61.164f));
        }
    }
}
