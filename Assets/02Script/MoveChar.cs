using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChar : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private Animator animator;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDir;
    private float moveSpeed;

    private void Awake()
    {
        moveSpeed = 2.0f;

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("MoveChar.cs failed");
        }
    }
    private void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = 0.0f;
        moveDir.z = Input.GetAxisRaw("Vertical");

        camForward = mainCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        camRight = mainCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        moveDir = moveDir.z * camForward + moveDir.x * camRight;
        moveDir.Normalize();

        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir;
        }
        animator.SetBool("IsWalk", moveDir != Vector3.zero);

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
