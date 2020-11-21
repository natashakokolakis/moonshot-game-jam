using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage;
    public GameObject projectile;
    public Transform projectileOrigin;

    EnemyAI enemyAI;
    GameObject player;
    PlayerHealth playerHealth;
    Collider enemyCollider;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyCollider = GetComponent<Collider>();
    }

    void MeleeAttack()
    {
        if ((player.transform.position - transform.position).magnitude <= enemyAI.attackRange)
        playerHealth.TakeDamage(attackDamage);
    }

    void DiveAttack()
    {
        enemyCollider.isTrigger = true;
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
        }
        enemyCollider.isTrigger = false;
    }
}
