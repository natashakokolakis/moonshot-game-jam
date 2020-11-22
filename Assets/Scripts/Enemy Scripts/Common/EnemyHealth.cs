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
    public int startingHealth = 20;
    public int currentHealth;
    
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
    
    Rigidbody rb;
    Animator anim;
    ChaseBehaviourPrefab chaseBehaviour;
    #endregion

    public void GetPushedBack(int damage, Vector3 travelDirection)
    {
        enemyMovementController.ApplyForce(travelDirection * damage * basePushback, ForceMode.Impulse);
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponentInChildren<Animator>();
        enemyMovementController = GetComponent<CharacterMovement>();
        chaseBehaviour = GetComponent<ChaseBehaviourPrefab>();
        rb = GetComponent<Rigidbody>();
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
        if (invincibilityCooldown || currentHealth <= 0)
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
        //anim.SetBool("isDead", true);
        anim.SetTrigger("Dead");
        rb.velocity = Vector3.zero;
        rb.Sleep();
        //chaseBehaviour.speed = 0;
        //chaseBehaviour.enabled = false;
        EventManagerNorth.StopListening("GolfBallSunk", Death);
        Destroy(gameObject, 4f);
    }


}
