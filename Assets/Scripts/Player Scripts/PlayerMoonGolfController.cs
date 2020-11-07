using ECM.Controllers;
using UnityEngine;

public class PlayerMoonGolfController : BaseCharacterController
{
    // groundMask is used to determine which layer is the ground in Unity
    public LayerMask groundMask = 1;

    // Used for basic attack
    public bool isAttacking = false;
    public float attackCooldownMax = 1.0f;
    protected float attackTimer;
    public Vector3 attackDirection = Vector3.zero;

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
            // Rotates towards mouse click for attack
            RotateTowards(attackDirection, true);
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

        if (isAttacking)
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
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            
            if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask.value))
            return;

            attackDirection = hitInfo.point;
            isAttacking = true;

        }

        return;
        
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
        }
    
    }

    //Sets components and values
    public override void Awake()
    {
        base.Awake();
        attackTimer = attackCooldownMax;
    }

    public override void FixedUpdate()
    {
        AttackCooldown();
        base.FixedUpdate();

    }
}


