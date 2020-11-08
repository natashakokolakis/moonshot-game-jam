﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossAttack : MonoBehaviour
{
    public GameObject minion;
    public Transform minionSpawnPoint; //center point of where minions will spawn
    public GameObject projectile;
    public Transform projectileOrigin;
    public GameObject chargeBeam;
    public int attackRange = 2;

    bool isAttacking = false;
    bool onCooldown;
    GameObject player;
    PlayerHealth playerHealth;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isAttacking == false && onCooldown == false && playerHealth.currentHealth > 0)
        {
            int num = Random.Range(0, 6);

            if (num == 0)
            {
                ChargedAttack();
            }
            if (num < 3 && num > 0)
            {
                SummonMinions();
            }
            else
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
                {
                    MeleeAttack();
                }
                RangedAttack();
            }
        }
    }

    void ChargedAttack()
    {
        //modify so that boss charges before activating beam
        isAttacking = true;
        onCooldown = true;
        chargeBeam.SetActive(true);

        StartCoroutine(AttackDuration(10f, 3f));
    }

    void SummonMinions()
    {
        isAttacking = true;
        onCooldown = true;

        //summon 5 minions at random positions
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(minion, minionSpawnPoint.position + randomPosition, minionSpawnPoint.rotation);
        }

        StartCoroutine(AttackDuration(2f, 7f));
    }

    void MeleeAttack()
    {
        isAttacking = true;
        onCooldown = true;
        DealDamage(5);

        StartCoroutine(AttackDuration(1f, 3f));
    }

    void RangedAttack()
    {
        isAttacking = true;
        onCooldown = true;
        Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);

        StartCoroutine(AttackDuration(1f, 3f));
    }

    IEnumerator AttackDuration(float duration, float minTimeBetweenAttack)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
        StartCoroutine(NextAttackDelay(minTimeBetweenAttack));
    }

    IEnumerator NextAttackDelay(float minTimeBetweenAttack)
    {
        yield return new WaitForSeconds(minTimeBetweenAttack);
        onCooldown = false;
    }

    void DealDamage(int attackDamage)
    {
        playerHealth.TakeDamage(attackDamage);
    }
}
