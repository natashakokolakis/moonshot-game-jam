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

    #endregion

    public void GetPushedBack(int damage, Vector3 travelDirection)
    {
        enemyMovementController.ApplyForce(travelDirection * damage * basePushback, ForceMode.Impulse);
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        enemyMovementController = this.GetComponent<CharacterMovement>();
    }

    public void TakeDamage(int amount, Vector3 travelDirection)
    {
        currentHealth -= amount;

        travelDirection = (this.transform.position - travelDirection).normalized;
        
        if (currentHealth <= 0)
        {
            Death();
        }

        GetPushedBack(amount, travelDirection);
    }

    void Death()
    {
        Destroy(gameObject);
    }


}
