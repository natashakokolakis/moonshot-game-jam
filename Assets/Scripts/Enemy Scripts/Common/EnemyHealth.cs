using System.Collections;
using System.Collections.Generic;
using ECM.Components;
using ECM.Controllers;
using UnityEngine;

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

    #endregion

    private Vector3 CalculatePushback(int damage, int basePushback)
    {
        return Vector3.zero;
    }

    public void GetPushedBack(Vector3 pushback)
    {
        enemyMovementController.ApplyForce(pushback);
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        enemyMovementController = this.GetComponent<CharacterMovement>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }


    }

    void Death()
    {
        Destroy(gameObject);
    }


}
