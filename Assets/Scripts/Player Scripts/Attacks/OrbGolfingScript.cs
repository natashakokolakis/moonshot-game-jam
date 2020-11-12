using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbGolfingScript : MonoBehaviour
{
    #region Variables and Dependencies

    public Rigidbody golfBallRB;
    public SphereCollider golfBallCollider;
    public TrailRenderer trailRenderer;
    public CapsuleCollider interactableCollider;

    public float basePower = 1f;
    public bool isStopped = true;



    #endregion

    public void ShootGolfBall(float golfPower, Vector3 direction)
    {
        trailRenderer.Clear();
        golfBallRB.velocity = Vector3.zero;
        direction = (direction + Vector3.up/10) * golfPower * basePower / 10;

        golfBallRB.isKinematic = false;
        golfBallRB.AddForce(direction, ForceMode.Impulse);

        isStopped = false;

    }

/*    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && golfBallRB.velocity.magnitude > 3.5f)
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(baseGolfBAllDamage*(int)Mathf.Clamp(golfBallRB.velocity.magnitude / 8, 2f, 4f) , transform.position);
            Debug.Log(baseGolfBAllDamage * (int)Mathf.Clamp(golfBallRB.velocity.magnitude / 8, 2f, 4f));
            //this.gameObject.SetActive(false);
            //golfBallCollider.enabled = false;
            trailRenderer.Clear();
        }
    }*/

    private void Awake()
    {
        golfBallRB = this.GetComponent<Rigidbody>();
        golfBallCollider = this.GetComponent<SphereCollider>();
        trailRenderer = this.GetComponent<TrailRenderer>();
        interactableCollider = this.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if (isStopped)
            return;

        if (golfBallRB.velocity.magnitude < 0.1f)
        {
            isStopped = true;
            golfBallRB.isKinematic = true;
        }

    }


}
