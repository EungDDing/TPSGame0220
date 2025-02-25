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

    private bool isRun;
    private bool isWalk;
    private bool isMove;
    private bool isAim;

    private void Awake()
    {
        isRun = false;
        isWalk = false;
        isMove = false;
        isAim = false;
        
        moveSpeed = 0.0f;

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("MoveChar.cs failed");
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isMove == true)
        {
            isWalk = false;
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        if (isWalk)
        {
            moveSpeed = 1.8f;
        }
        else if (isRun)
        {
            moveSpeed = 5.0f;
        }
        else
        {
            moveSpeed = 0.0f;
        }

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

        isMove = (moveDir != Vector3.zero);
        isWalk = (moveDir != Vector3.zero);

        animator.SetFloat("Speed", moveSpeed);
        
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
