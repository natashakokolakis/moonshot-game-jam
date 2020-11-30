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


    /*[Header("Weaknesses")]
    [SerializeField]
    private int meleeWeakness = 1;
    [SerializeField]
    private int rangedWeakness = 1;
    [SerializeField]
    private int specialWeakness = 1;*/


    public int basePushback = 50;
    public float invincibilityLength = .5f; 
    [SerializeField] private bool invincibilityCooldown = false;
    
    Rigidbody rb;
    Animator anim;
    ChaseBehaviourPrefab chaseBehaviour;
    BossAttack bossAttack;

    [FMODUnity.EventRef]
    public string EnemyImpactEvent = "";

    [FMODUnity.EventRef]
    public string EnemyHurtEvent = "";

    #endregion


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponentInChildren<Animator>();
        enemyMovementController = GetComponent<CharacterMovement>();
        chaseBehaviour = GetComponent<ChaseBehaviourPrefab>();
        rb = GetComponent<Rigidbody>();

        if (gameObject.CompareTag("Boss"))
        {
            bossAttack = GetComponentInChildren<BossAttack>();
        }
    }

    private void OnEnable()
    {
        EventManagerNorth.StartListening("GolfBallSunk", Death);
    }

    private void OnDisable()
    {
        EventManagerNorth.StopListening("GolfBallSunk", Death);
    }

    private void OnDestroy()
    {
        EventManagerNorth.StopListening("GolfBallSunk", Death);
    }

    public void TakeDamage(int amount, Vector3 travelDirection)
    {
        if (invincibilityCooldown || currentHealth <= 0)
            return;

        PlayHurtSound(amount);
        FMODUnity.RuntimeManager.PlayOneShot(EnemyHurtEvent, transform.position);

        invincibilityCooldown = true;
        currentHealth -= amount;
        anim.SetTrigger("isHit");

        travelDirection = (this.transform.position - travelDirection).normalized;

        if (currentHealth <= 0)
        {
            Death();
        }

        GetPushedBack(amount, travelDirection);


        if (gameObject.CompareTag("Boss"))
        {
            if (currentHealth < startingHealth / 2 && !bossAttack.isEnraged)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Boss/BossEnraged", transform.position);
                anim.SetTrigger("Enraged");
                bossAttack.isEnraged = true;
            }
        }
        StartCoroutine(InvincibilityAfterDamage(invincibilityLength));
    }

    public void GetPushedBack(int damage, Vector3 travelDirection)
    {
        enemyMovementController.ApplyForce(travelDirection * damage * basePushback, ForceMode.Impulse);
    }

    IEnumerator InvincibilityAfterDamage(float time)
    {
        yield return new WaitForSeconds(time);
        invincibilityCooldown = false;
    }

    void Death()
    {
        //disappear (use MM feedbacks) then destroy self at end of animation
        anim.SetTrigger("Dead");
        rb.velocity = Vector3.zero;
        rb.Sleep();
        chaseBehaviour.chaseTarget = null;

        if (gameObject.CompareTag("Boss"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Boss/BossDeath", transform.position);
            EventManagerNorth.StopListening("GolfBallSunk", Death);
            EventManagerNorth.TriggerEvent("GolfBallSunk");
        }
        EventManagerNorth.StopListening("GolfBallSunk", Death);
    }

    public void PlayHurtSound(float damageAmount)
    {
        FMOD.Studio.EventInstance enemyHurtSound = FMODUnity.RuntimeManager.CreateInstance(EnemyImpactEvent);


        enemyHurtSound.setParameterByName("EnemyDamageAmount", damageAmount, true);
        enemyHurtSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        enemyHurtSound.start();
        enemyHurtSound.release();
    }


}
