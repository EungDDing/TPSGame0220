using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChar : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private Animator animator;
    private ShootBullet shootBullet;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDir;
    private Vector3 playerForward; 
    private Ray ray;
    [SerializeField] private float moveSpeed;

    private bool isRun;
    private bool isMove;
    private bool isAiming;

    public float MoveSpeed
    {
        get => moveSpeed;
    }

    private void Awake()
    {
        isAiming = false;
        isRun = false;
        isMove = false;
        
        moveSpeed = 0.0f;

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("MoveChar.cs TryGetComponent<Animator> failed");
        }
        if (!TryGetComponent<ShootBullet>(out shootBullet))
        {
            Debug.Log("MoveChar.cs TryGetComponent<ShootBullet> failed");
        }

        shootBullet.OnAimingChange += AimMove;
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        playerForward = ray.direction;
        playerForward.y = 0.0f; 

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

        isMove = (moveDir != Vector3.zero);
        isRun = isMove && Input.GetKey(KeyCode.LeftShift);

        moveSpeed = isAiming ? 0.8f : (isRun ? 5.0f : (isMove ? 1.8f : 0.0f));
 
        if (isMove)
        {
            playerForward = moveDir;
            transform.forward = playerForward;
        }

        if (isAiming)
        {
            transform.forward = playerForward;
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        animator.SetFloat("Speed", moveSpeed);
    }
    public void AimMove(bool newAimingState)
    {
        isAiming = newAimingState;
    }
}
