using System.Collections;
using System.Collections.Generic;
using ECM.Components;
using ECM.Controllers;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    #region Stats and Dependencies

    [Header("Health Stats")]
    [SerializeField]
    private int startingHealth = 20;
    [SerializeField]
    private int currentHealth;
    
    // Used for pushing back enemy
    private CharacterMovement enemyMovementController;


    [Header("Weaknesses")]
    [SerializeField]
    private int meleeWeakness = 1;
    [SerializeField]
    private int rangedWeakness = 1;
    [SerializeField]
    private int specialWeakness = 1;


    public int basePushback = 50;
    public bool invincibilityCooldown = false;

    Animator anim;
    #endregion

    public void GetPushedBack(int damage, Vector3 travelDirection)
    {
        enemyMovementController.ApplyForce(travelDirection * damage * basePushback, ForceMode.Impulse);
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        enemyMovementController = this.GetComponent<CharacterMovement>();
    }

    private void OnEnable()
    {
        EventManagerNorth.StartListening("GolfBallSunk", Death);
    }

    private void OnDisable()
    {
        EventManagerNorth.StopListening("GolfBallSunk", Death);
    }

    IEnumerator InvincibilityAfterDamage()
    {
        yield return new WaitForSeconds(.5f);
        invincibilityCooldown = false;
    }

    public void TakeDamage(int amount, Vector3 travelDirection)
    {
        if (invincibilityCooldown)
            return;

        invincibilityCooldown = true;
        currentHealth -= amount;
        anim.SetBool("isHit", true);

        travelDirection = (this.transform.position - travelDirection).normalized;
        
        if (currentHealth <= 0)
        {
            Death();
        }

        GetPushedBack(amount, travelDirection);

        StartCoroutine("InvincibilityAfterDamage");
    }

    void Death()
    {
        //disappear (use MM feedbacks) then destroy self at end of animation
        anim.SetBool("isDead", true);
        EventManagerNorth.StopListening("GolfBallSunk", Death);
        Destroy(gameObject, 4f);
    }


}
