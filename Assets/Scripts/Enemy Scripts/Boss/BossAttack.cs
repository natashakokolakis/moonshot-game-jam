using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//add method to check if player in range, then determine whether to melee or ranged attack

public class BossAttack : MonoBehaviour
{
    public GameObject minion;
    public Transform minionSpawnPoint; //center point of where minions will spawn
    public GameObject projectile;
    public Transform projectileOrigin;

    bool isAttacking = false;
    bool onCooldown;
    //GameObject player;
    //PlayerHealth playerHealth;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isAttacking == false && onCooldown == false /* && playerHealth.currentHealth > 0 */)
        {
            int num = Random.Range(0, 5);

            if (num == 0)
            {
                ChargedAttack();
            }
            if (num < 3)
            {
                SummonMinions();
            }
            else
            {
                RangedAttack();
            }
        }
    }

    void ChargedAttack()
    {
        //add damage script to projectile so that it only damages player if it hits
        isAttacking = true;
        onCooldown = true;

        StartCoroutine(AttackDuration(10f));
        StartCoroutine(NextAttackDelay(3f));
    }

    void SummonMinions()
    {
        isAttacking = true;
        onCooldown = true;

        //summon 5 minions at random positions
        for (int i=0; i < 5; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(minion, minionSpawnPoint.position + randomPosition, minionSpawnPoint.rotation);
        }

        StartCoroutine(AttackDuration(2f));
        StartCoroutine(NextAttackDelay(7f));
    }

    void RangedAttack()
    {
        //add damage script to projectile so that it only damages player if it hits
        isAttacking = true;
        onCooldown = true;
        Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);

        StartCoroutine(AttackDuration(1f));
        StartCoroutine(NextAttackDelay(3f));
    }

    IEnumerator AttackDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    IEnumerator NextAttackDelay (float minTimeBetweenAttack)
    {
        yield return new WaitForSeconds(minTimeBetweenAttack);
        onCooldown = false;
    }
}
