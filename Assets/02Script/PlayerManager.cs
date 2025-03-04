using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private Animator animator;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDir;
    private Vector3 playerForward; 
    private Ray ray;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject aimObject;
    [SerializeField] private float aimDistance;
    
    private bool isRun;
    private bool isMove;
    private bool isAimingMove;
    private bool isAim;
    private bool isReload;

    public delegate void IsAimingChange(bool isAiming);
    public event IsAimingChange OnAimingChange;

    public float MoveSpeed
    {
        get => moveSpeed;
    }

    private void Awake()
    {
        isAimingMove = false;
        isRun = false;
        isMove = false;
        isAim = false;
        isReload = false;

        aimDistance = 20.0f;
        moveSpeed = 0.0f;

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("MoveChar.cs TryGetComponent<Animator> failed");
        }
    }
    private void Update()
    {
        PlayerMove();
        isRun = isMove && Input.GetKey(KeyCode.LeftShift);
        isAimingMove = isMove && Input.GetMouseButton(1);

        moveSpeed = isAimingMove ? 0.8f : (isRun ? 5.0f : (isMove ? 1.8f : 0.0f));

        bool newAimState = Input.GetMouseButton(1) && (!isReload);


        if (newAimState != isAim)
        {
            isAim = newAimState;
            OnAimingChange?.Invoke(isAim);
        }
        
        
            if (isMove)
            {
                playerForward = moveDir;
                transform.forward = playerForward;
            }


            if (isAim)
            {
                animator.SetLayerWeight(1, 1);

                Vector3 targetPos = Vector3.zero;
                Transform camTransform = mainCam.transform;
                RaycastHit hit;

                if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity))
                {
                    targetPos = hit.point;
                    aimObject.transform.position = hit.point;
                }
                else
                {
                    targetPos = camTransform.position + camTransform.forward * aimDistance;
                    aimObject.transform.position = camTransform.position + camTransform.forward * aimDistance;
                }

                Vector3 targetAim = targetPos;
                targetAim.y = transform.position.y;
                Vector3 aimDir = (targetAim - transform.position).normalized;

                transform.forward = aimDir;

                if (Input.GetMouseButton(0))
                {
                    animator.SetBool("Fire", true);
                }
                else
                {
                    animator.SetBool("Fire", false);
                }
            }
            else
            {
                animator.SetLayerWeight(1, 0);
            }
        

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        animator.SetFloat("Speed", moveSpeed);
    }

    private void PlayerMove()
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

        isMove = (moveDir != Vector3.zero);
    }
}
