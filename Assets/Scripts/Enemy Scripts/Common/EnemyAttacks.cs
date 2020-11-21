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
    Collider enemyCollider;
    Animator anim;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyCollider = GetComponent<Collider>();
        anim = GetComponent<Animator>();
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
        while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            enemyCollider.isTrigger = true;
        }
        enemyCollider.isTrigger = false;
    }

    void RangedAttack()
    {
        Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject == player)
        {
            playerHealth.TakeDamage(attackDamage);
            enemyCollider.isTrigger = false;
        }
    }
}
