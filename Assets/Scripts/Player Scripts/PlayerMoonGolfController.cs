﻿using ECM.Controllers;
using UnityEngine;

public class PlayerMoonGolfController : BaseCharacterController
{
    #region VARIABLES AND DEPENDENCIES

    // groundMask is used to determine which layer is the ground in Unity
    public LayerMask groundMask = 1;

    // Turns off inputs when dead
    [HideInInspector]
    public bool isDead = false;
    
    [Header("Attack")]
    // Used for basic attack
    public bool isAttacking = false;
    public float attackCooldownMax = 1.0f;
    protected float attackTimer;
    public Vector3 attackDirection = Vector3.zero;

    public BoxCollider meleeBoxCollider;
    public Animator meleeAnimator;

    [Header("Ranged")]
    public bool isAiming = false;
    public float golfPowerRate = 1f;
    public float golfPower = 0f;
    public float golfPowerMin = 0f;
    public float golfPowerMAX = 33f;
    public GameObject golfBallObject;
    public GolfBallAttack golfBallAttack;
    public GameObject aimLine;
    public TrailRenderer golfBallTrail;

    [Header("Orb Golf")]
    public GameObject orbBallObject;
    public CapsuleCollider orbInteractionCollider;

    public AOEAttackHandler aoeHandler;
    public bool isInAOE = false;

    #endregion

    #region Custom Methods

    private void MeleeAttack()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask.value))
            return;

        attackDirection = Vector3.ProjectOnPlane(hitInfo.point - transform.position, transform.up);
        moveDirection = Vector3.zero;
        isAttacking = true;
        meleeBoxCollider.enabled = true;
        meleeAnimator.SetTrigger("MeleeAttack");
    }

    private void RangedAttack()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask.value))
            return;

        moveDirection = Vector3.zero;
        attackDirection = Vector3.ProjectOnPlane(hitInfo.point - transform.position, transform.up).normalized;
        
        movement.Rotate(attackDirection, 900, false);
        

        golfPower += golfPowerRate * Time.deltaTime;

        if (golfPower >= golfPowerMAX)
            golfPowerRate = -golfPowerRate;

        if (golfPower <= 0)
            golfPowerRate = -golfPowerRate;
        
    }

    private void AOEAttack()
    {
        moveDirection = Vector3.zero;
        isInAOE = true;
        //pause = !pause;
        StartCoroutine(aoeHandler.AOESpecial());

    }

    private void AttackCooldown()
    {
        if (isPaused)
            return;

        if (attackTimer < attackCooldownMax && isAttacking)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0f;
            isAttacking = false;
            meleeBoxCollider.enabled = false;
        }

    }

    private void OnTriggerStay(Collider other)
    {
            if (Input.GetKey(KeyCode.E))
            {
            if (other.CompareTag("GolfBall"))
            {
                other.GetComponent<OrbGolfingScript>().SetUpGolfMode();
                this.gameObject.SetActive(false);
            }
            }
    }

    #endregion

    #region Override Methods

    protected override void Animate()
    {
        
    }

    protected override void UpdateRotation()
    {
        if (useRootMotion && applyRootMotion && useRootMotionRotation)
        {
            // Use animation angular velocity to rotate character

            Quaternion q = Quaternion.Euler(Vector3.Project(rootMotionController.animAngularVelocity, transform.up));

            movement.rotation *= q;
        }
        else if (isAttacking)
        {
            // Rotate towards player click

            movement.Rotate(attackDirection, 900, false);

        }
        else
        {
            // Rotate towards movement direction (input)

            RotateTowardsMoveDirection();
        }
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
            pause = !pause;

        // Handle user input

        //Do nothing if dead/ attacking

        if (isAttacking | isDead | isInAOE)
            return;


        moveDirection = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0.0f,
            z = Input.GetAxisRaw("Vertical")
        };

        jump = Input.GetButton("Jump");

        crouch = Input.GetKey(KeyCode.C);

        // Basic attack. Confirms where user clicked and sets isAttacking to true

        if (Input.GetButtonDown("Fire1"))
        {
            MeleeAttack();
            aimLine.SetActive(false);
            golfPower = 0;
            golfPowerRate = Mathf.Abs(golfPowerRate);
        }

        if (Input.GetButton("Fire2") & !isAttacking)
        {
            RangedAttack();
            isAiming = true;
            aimLine.SetActive(true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
         {
            AOEAttack();
         }

        if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;

            isAttacking = true;
            moveDirection = Vector3.zero;
            golfBallObject.SetActive(true);
            golfBallObject.transform.position = transform.localPosition + attackDirection.normalized + new Vector3(0, .5f, 0);
            golfPower = Mathf.Clamp(golfPower, 0, golfPowerMAX - 1);
            golfBallAttack.ShootGolfBall(golfPower, attackDirection);
            aimLine.SetActive(false);
            isAttacking = true;
            meleeAnimator.SetTrigger("MeleeAttack");

            golfPower = 0;
            golfPowerRate = Mathf.Abs(golfPowerRate);
        }

        return;
        
    }

    #endregion

    #region MonoBehaviours

    public override void Awake()
    {
        base.Awake();
        attackTimer = attackCooldownMax;
        meleeAnimator = transform.Find("Player").transform.Find("GolfClub").GetComponent<Animator>();
        
    }

    public override void FixedUpdate()
    {
        AttackCooldown();
        base.FixedUpdate();

    }

    #endregion
}


