using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private Animator animator;
    private Transform spine;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDir;
    private Vector3 playerForward;

    private Vector3 aimDir;
    private Vector3 targetPos;

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

        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
    }
    private void Update()
    {
        PlayerMove();
        isRun = isMove && Input.GetKey(KeyCode.LeftShift) && !isReload;
        isAimingMove = isMove && Input.GetMouseButton(1) && !isReload;

        moveSpeed = isAimingMove ? 0.8f : (isRun ? 5.0f : (isMove ? 1.8f : 0.0f));

        if (Input.GetKeyDown(KeyCode.R) && !isReload)
        {
            StartCoroutine(Reload());
        }

        bool newAimState = Input.GetMouseButton(1) && !isReload;
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

        if (!isReload)
        {
            if (isAim)
            {
                animator.SetBool("Aim", true);
                animator.SetLayerWeight(1, 1);
                PlayerAim();
            }
            else
            {
                animator.SetBool("Aim", false);
                animator.SetLayerWeight(1, 0);
            }
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

    private void PlayerAim()
    {
    
        targetPos = Vector3.zero;
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
        aimDir = (targetAim - transform.position).normalized;

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
    private IEnumerator Reload()
    {
        isReload = true;
        animator.SetLayerWeight(1, 1);
        animator.SetTrigger("Reload");
        
        Debug.Log("Reload Trigger Set");

        yield return new WaitForSeconds(4.0f);

        animator.SetLayerWeight(1, 0);
        isReload = false;

        Debug.Log("Reload Finished");
    }
}
