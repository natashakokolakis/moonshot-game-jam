using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public int attackDamage = 3;
    public GameObject projectile;
    public Transform projectileOrigin;

    EnemyAI enemyAI;
    ChaseBehaviourPrefab chaseBehaviour;
    GameObject player;
    PlayerHealth playerHealth;
    SphereCollider diveCollider;
    //Collider parentCollider;
    Animator anim;
    //float prevStoppingDistance;
    float diveSpeed;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        chaseBehaviour = GetComponentInParent<ChaseBehaviourPrefab>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        diveCollider = GetComponentInChildren<SphereCollider>();
        //parentCollider = GetComponentInParent<Collider>();
        anim = GetComponent<Animator>();
        //prevStoppingDistance = chaseBehaviour.stoppingDistance;
        diveSpeed = chaseBehaviour.speed * 2;
    }

    void MeleeAttack()
    {
        if ((player.transform.position - transform.position).magnitude <= enemyAI.attackRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void DiveAttack()
    {
        diveCollider.enabled = true;
        //StartCoroutine(IncreaseSpeed());
        //diveCollider.isTrigger = true;
        //parentCollider.enabled = false;
        //chaseBehaviour.speed *= 2;
        //chaseBehaviour.stoppingDistance = 0;
    }

    void RangedAttack()
    {
        Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
    }

    /*public void OnChildTriggerEnter(Collider other)
    {
         if (other.gameObject == player)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }*/

    public void OnChildCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    /*IEnumerator IncreaseSpeed()
    {
        transform.parent.position += Vector3.forward * diveSpeed * Time.deltaTime;
        yield return new WaitForSeconds(2);
    }*/

    void ResetCollider()
    {
        diveCollider.enabled = false;
        //StopCoroutine(IncreaseSpeed());
        //diveCollider.isTrigger = false;
        //parentCollider.enabled = true;
        //chaseBehaviour.speed = 0;
        //chaseBehaviour.stoppingDistance = prevStoppingDistance;
    }
}
