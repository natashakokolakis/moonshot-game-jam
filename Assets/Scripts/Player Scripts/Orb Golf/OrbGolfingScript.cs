using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbGolfingScript : MonoBehaviour
{
    #region Variables and Dependencies

    // Components
    public Rigidbody golfBallRB;
    public SphereCollider golfBallCollider;
    public TrailRenderer trailRenderer;
    public CapsuleCollider interactableCollider;

    // Stats
    public float basePower = 1f;
    public bool isStopped = true;

    // Cooldown to check when ball has stopped moving
    public float velCheckTimer = 0f;
    public float velCheckTimerStart = 1f;

    // For aiming
    public LayerMask groundMask = 1;
    public Vector3 attackDirection = Vector3.zero;

    public float golfPowerRate = 1f;
    public float golfPower = 0f;
    public float golfPowerMin = 0f;
    public float golfPowerMAX = 33f;

    #endregion

    // Rotates player while aiming
    public void Rotate(Vector3 direction, float angularSpeed, bool onlyLateral = true)
    {
        if (onlyLateral)
            direction = Vector3.ProjectOnPlane(direction, transform.up);

        if (direction.sqrMagnitude < 0.0001f)
            return;

        var targetRotation = Quaternion.LookRotation(direction, transform.up);
        var newRotation = Quaternion.Slerp(golfBallRB.rotation, targetRotation,
            angularSpeed * Mathf.Deg2Rad * Time.deltaTime);

        golfBallRB.MoveRotation(newRotation);
    }

    

    public void ShootGolfBall(float golfPower, Vector3 direction)
    {
        trailRenderer.Clear();
        golfBallRB.velocity = Vector3.zero;
        direction = direction * golfPower * basePower;

        golfBallRB.isKinematic = false;
        golfBallRB.AddForce(direction, ForceMode.VelocityChange);

        isStopped = false;

    }

    public void AimGolfBall()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask.value))
            return;

        attackDirection = Vector3.ProjectOnPlane(hitInfo.point - transform.position, transform.up).normalized;
        Rotate(attackDirection, 900, false);


        golfPower += golfPowerRate * Time.deltaTime;

        if (golfPower >= golfPowerMAX)
            golfPowerRate = -golfPowerRate;

        if (golfPower <= 0)
            golfPowerRate = -golfPowerRate;
    }

    public void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
            ShootGolfBall(golfPower, attackDirection);

    }

    #region Monobehaviours

    private void Awake()
    {
        golfBallRB = this.GetComponent<Rigidbody>();
        golfBallCollider = this.GetComponent<SphereCollider>();
        trailRenderer = this.GetComponent<TrailRenderer>();
        interactableCollider = this.GetComponent<CapsuleCollider>();

        golfBallRB.maxAngularVelocity = 100f;
    }

    private void FixedUpdate()
    {
        if (isStopped)
            return;

        if (velCheckTimer >= velCheckTimerStart)
        {
            if (golfBallRB.velocity.magnitude < 0.06f)
            {
                isStopped = true;
                velCheckTimer = 0f;
                golfBallRB.isKinematic = true;
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }

        velCheckTimer += Time.deltaTime;

    }

    private void Update()
    {
        AimGolfBall();
        HandleInput();
    }

    #endregion
}
