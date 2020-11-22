using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator anim;
    PlayerMoonGolfController playerController;
    PlayerHealth playerHealth;
    float golfPower;
    Vector3 speedValue;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerMoonGolfController>();
        playerHealth = GetComponent<PlayerHealth>();
        golfPower = playerController.golfPower / playerController.golfPowerMAX;
    }

    private void Update()
    {
        CheckForEnemies();
        CheckMovment();
    }

    void CheckForEnemies()
    {
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            anim.SetBool("enemiesPresent", true);
        }
        else
        {
            anim.SetBool("enemiesPresent", false);
        }
    }

    void CheckMovment()
    {
        speedValue = playerController.moveDirection.normalized;
        if (speedValue != new Vector3(0, 0, 0))
        {
            anim.SetFloat("Speed", 1);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    void MeleeAttack()
    {
        anim.SetTrigger("melee");
    }

    void RangedAttack()
    {
        if (golfPower < 0.3f)
        {
            anim.SetTrigger("weakAttack");
        }
        else if (golfPower < 0.7f)
        {
            anim.SetTrigger("mediumAttack");
        }
        else
        {
            anim.SetTrigger("strongAttack");
        }
    }

    void Golfing()
    {
        if (golfPower < 0.1f)
        {
            anim.SetTrigger("golfFail");
        }
        else if (golfPower < 0.3f)
        {
            anim.SetTrigger("golfPut");
        }
        else if (golfPower < 0.7f)
        {
            anim.SetTrigger("golfChip");
        }
        else
        {
            anim.SetTrigger("golfDrive");
        }
    }

    void Damaged()
    {
        anim.SetTrigger("damaged");
    }

    void Death()
    {
        anim.SetBool("isDead", true);
    }

}
