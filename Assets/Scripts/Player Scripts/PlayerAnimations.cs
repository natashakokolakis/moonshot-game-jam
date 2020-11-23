using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator anim;
    PlayerMoonGolfController playerController;
    float golfPower;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerMoonGolfController>();
    }

    private void Update()
    {
        CheckForEnemies();
        CheckMovment();
    }

    void CheckForEnemies()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
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
        if (playerController.moveDirection != new Vector3(0, 0, 0))
        {
            anim.SetFloat("Speed", 1);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    public void MeleeAttack()
    {
        anim.SetTrigger("melee");
    }

    public void EnterAttackMode()
    {
        anim.SetBool("attackMode", true);
    }

    public void RangedAttack(float power, float maxPower)
    {
        anim.SetBool("attackMode", false);
        golfPower = power / maxPower;
        if (golfPower < 0.3f)
        {
            anim.SetInteger("attackStrength", 0);
        }
        else if (golfPower < 0.7f)
        {
            anim.SetInteger("attackStrength", 1);
        }
        else
        {
            anim.SetInteger("attackStrength", 2);
        }
    }

    public void EnterGolfMode()
    {
        anim.SetBool("golfMode", true);
    }

    //not called properly yet, waiting until dummy model no longer used for golf mode
    public void GolfSwing(float selectedPower, float maxPower)
    {
        anim.SetBool("golfMode", false);
        golfPower = selectedPower / maxPower;
        if (golfPower < 0.1f)
        {
            anim.SetInteger("golfStrength", 0);
        }
        else if (golfPower < 0.3f)
        {
            anim.SetInteger("golfStrength", 1);
        }
        else if (golfPower < 0.7f)
        {
            anim.SetInteger("golfStrength", 2);
        }
        else
        {
            anim.SetInteger("golfStrength", 3);
        }
    }

    //golf mode and range attack cancel doesn't exist yet, not used there
    public void CancelAiming()
    {
        if (anim.GetBool("attackMode") == true)
        {
            anim.SetBool("attackMode", false);
        }
        else 
        {
            anim.SetBool("golfMode", false);
        }
        anim.SetTrigger("cancel");
    }

    public void GolfInHole()
    {
        anim.SetTrigger("golfInHole");
    }

    //not called anywhere yet
    public void PickUp()
    {
        anim.SetTrigger("pickUp"); 
    }

    public void Damaged(float damageValue)
    {
        if (damageValue < 5)
        {
            anim.SetTrigger("smallDamage");
        }
        else
        {
            anim.SetTrigger("bigDamage");
        }
    }

    public void Death()
    {
        anim.SetBool("isDead", true);
    }

}
