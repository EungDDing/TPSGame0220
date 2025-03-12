using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private CharacterController controller;
    private Animator animator;
    private Transform spine;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDir;
    private Vector3 playerForward;

    private Vector3 aimDir;
    private Vector3 targetPos;

    private RaycastHit hit;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject aimObject;
    [SerializeField] private float aimDistance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float maxShootDelay = 0.07f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioClip shootingSound;
    private AudioSource audioSource;
    private float shootDelay;
    private int hasBullet = 120;
    private int maxBullet = 30;
    private int currentBullet = 0;

    private bool isRun;
    private bool isMove;
    private bool isAimingMove;
    private bool isAim;
    private bool isReload;
    private bool isEmpty;
    
    private bool isEnable;

    private bool isAimCollision;
    private float disableAimTime = 0.0f;
    private float disableTime = 1.5f;

    private int currentHP;
    private int maxHP = 300;

    public delegate void IsAimingChange(bool isAiming);
    public event IsAimingChange OnAimingChange;
    public delegate void BulletCountChange(int bulletCount);
    public event BulletCountChange OnCurBulletCountChange;
    public event BulletCountChange OnHasBulletCountChange;
    public delegate void HPChange(int hp);
    public event HPChange OnHPChange;
    public float MoveSpeed
    {
        get => moveSpeed;
    }

    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = value;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }
            OnHPChange?.Invoke(currentHP);
        }
    }
    private void Awake()
    {
        isAimingMove = false;
        isRun = false;
        isMove = false;
        isAim = false;
        isReload = false;
        isEmpty = false;
        isAimCollision = false;

        isEnable = true;

        aimDistance = 20.0f;
        moveSpeed = 0.0f;

        TryGetComponent<CharacterController>(out controller);

        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("MoveChar.cs TryGetComponent<Animator> failed");
        }

        TryGetComponent<AudioSource>(out audioSource);
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);

        shootDelay = 0.0f;
        InitBullet();
        InitHP();
    }
    private void Update()
    {
        PlayerMove();
        isRun = isMove && Input.GetKey(KeyCode.LeftShift) && !isReload;
        isAimingMove = isMove && Input.GetMouseButton(1) && !isReload && !isAimCollision;

        moveSpeed = isAimingMove ? 0.8f : (isRun ? 5.0f : (isMove ? 1.8f : 0.0f));

        PlayerReload();

        CheckAimCollision();
        
        PlayerAim();

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
                Aiming();
            }
            else
            {
                animator.SetBool("Aim", false);
                animator.SetLayerWeight(1, 0);
            }
        }

        if (isEnable)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            moveSpeed = 0.0f;
            animator.SetFloat("Speed", moveSpeed);
        }
    }
    private void LateUpdate()
    {
        if (isAim)
        {
            spine = animator.GetBoneTransform(HumanBodyBones.Spine);
            spine.transform.LookAt(aimObject.transform.position);
        }
    }
    private void PlayerMove()
    {
        if (!isEnable)
        {
            return;
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

        isMove = (moveDir != Vector3.zero);
    }
    private void PlayerReload()
    {
        if (!isEnable)
            return;

        if (Input.GetKeyDown(KeyCode.R) && !isReload)
        {
            StartCoroutine(Reload());
        }
    }
    private void PlayerAim()
    {
        if (!isEnable || isAimCollision)
            return;

        bool newAimState = Input.GetMouseButton(1) && !isReload;
        if (newAimState != isAim)
        {
            isAim = newAimState;
            OnAimingChange?.Invoke(isAim);
        }
    }
    private void Aiming()
    {
        targetPos = Vector3.zero;
        Transform camTransform = mainCam.transform;
        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, layerMask))
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

        if (Input.GetMouseButton(0) && !isEmpty)
        {
            animator.SetBool("Fire", true);
            FireBullet();
        }
        else
        {
            animator.SetBool("Fire", false);
        }
    }
    private void FireBullet()
    {
        shootDelay += Time.deltaTime;
        if (shootDelay < maxShootDelay)
            return;
        if (currentBullet <= 0)
        {
            StartCoroutine("Reload");
            return;
        }
        currentBullet -= 1;
        OnCurBulletCountChange?.Invoke(currentBullet);

        PlayWeaponSound(shootingSound);

        if (hasBullet == 0 && currentBullet == 0)
            isEmpty = true;

        shootDelay = 0;
        // Instantiate(bullet, spawnPos.position, Quaternion.identity);
        PoolManager.instance.FireBullet(spawnPos.position, aimObject.transform.position, gameObject, 10, 10.0f);
    }
    private IEnumerator Reload()
    {
        isReload = true;
        animator.SetLayerWeight(1, 1);
        animator.SetTrigger("Reload");
        
        Debug.Log("Reload Trigger Set");

        yield return new WaitForSeconds(4.0f);

        InitBullet();
        animator.SetLayerWeight(1, 0);
        isReload = false;

        Debug.Log("Reload Finished");
    }
    private void InitBullet()
    {
        hasBullet = hasBullet + currentBullet;
        if (hasBullet == 0)
        {
            return;
        }
        else
        {
            if (hasBullet < maxBullet)
            {
                maxBullet = hasBullet;
                currentBullet = hasBullet;
            }
            else
            {
                maxBullet = 30;
                currentBullet = maxBullet;
            }
        }
        
        hasBullet = hasBullet - maxBullet;
        
        OnHasBulletCountChange?.Invoke(hasBullet);
        OnCurBulletCountChange?.Invoke(currentBullet);
    }
    private void InitHP()
    {
        CurrentHP = maxHP;
    }
    private void PlayWeaponSound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }
    public void SetEnable(bool state)
    {
        isEnable = state;
    }
    private void CheckAimCollision()
    {
        Vector3 rayDir;
        rayDir = targetPos - spawnPos.position;
        float distance = Vector3.Distance(spawnPos.position, targetPos);

        if (distance < 0.8f)
        {
            if (!isAimCollision)
            {
                isAimCollision = true;
                disableAimTime = Time.time + disableTime;
                isAim = false;
                OnAimingChange?.Invoke(isAim);
            }
        }
        else if (isAimCollision && Time.time >= disableAimTime)
        {
            isAimCollision = false;
        }
    }
    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        CurrentHP -= damage;
    }
    public void GetAmmo()
    {

    }
}
