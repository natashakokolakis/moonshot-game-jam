using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public int attackDamage = 3;
    public GameObject projectile;
    public Transform projectileOrigin;

    EnemyAI enemyAI;
    GameObject player;
    PlayerHealth playerHealth;
    SphereCollider diveCollider;
    CapsuleCollider diveTrigger;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        diveCollider = GetComponentInChildren<SphereCollider>();
        diveTrigger = GetComponentInChildren<CapsuleCollider>();
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
        diveTrigger.enabled = true;
    }

    void RangedAttack()
    {
        if (projectile != null && projectileOrigin != null)
            Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
    }

    public void OnChildTriggerEnter(Collider other)
    {
         if (other.gameObject == player)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void ResetCollider()
    {
        diveCollider.enabled = false;
        diveTrigger.enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(transform.root.gameObject, 1);
    }
}
