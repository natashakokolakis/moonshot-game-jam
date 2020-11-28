using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallAttack : MonoBehaviour
{
    #region Variables and Dependencies

    private Rigidbody golfBallRB;
    private SphereCollider golfBallCollider;
    public float basePower = 1f;
    public int baseGolfBAllDamage = 1;
    private Vector3 currentVelocity = Vector3.zero;
    private TrailRenderer trailRenderer;
    public float trailTimer = 3f;

    [HideInInspector] public EnemyHealth enemyTarget;

    #endregion

    public void ShootGolfBall(float golfPower, Vector3 direction)
    {
        trailRenderer.Clear();
        golfBallRB.velocity = Vector3.zero;
        golfBallCollider.enabled = true;
        direction = (direction + Vector3.up/10) * golfPower * basePower / 10;
        golfBallRB.AddForce(direction, ForceMode.VelocityChange);

    }



    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) && golfBallRB.velocity.magnitude > 3.5f)
        {
            enemyTarget = collision.gameObject.GetComponent<EnemyHealth>();
            trailRenderer.Clear();
        }
    }

    private void Awake()
    {
        golfBallRB = this.GetComponent<Rigidbody>();
        golfBallCollider = this.GetComponent<SphereCollider>();
        trailRenderer = this.GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if (!enemyTarget)
            return;

        enemyTarget.TakeDamage(baseGolfBAllDamage * (int)Mathf.Clamp(golfBallRB.velocity.magnitude / 7, 2f, 4f), transform.position);
        enemyTarget = null;
    }

}
