using ECM.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth= 1;
    public bool isDead = false;
    //public Slider healthSlider;

    PlayerMoonGolfController playerController;
    CharacterMovement characterMovement;
    PlayerAnimations animate;

    private void Awake()
    {
        playerController = GetComponent<PlayerMoonGolfController>();
        currentHealth = maxHealth;
        animate = GetComponent<PlayerAnimations>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        animate.Damaged(amount);
        //healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        isDead = true;
        animate.Death();
        playerController.moveDirection = Vector3.zero;
        playerController.isDead = true;
    }
}
