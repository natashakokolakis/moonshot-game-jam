﻿using ECM.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth= 1;
    public bool isDead = false;
    
    Slider healthSlider;
    PlayerMoonGolfController playerController;
    CharacterMovement characterMovement;
    PlayerAnimations animate;

    private void Awake()
    {
        playerController = GetComponent<PlayerMoonGolfController>();
        healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        animate = GetComponent<PlayerAnimations>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
        else
        {
            animate.Damaged(amount);
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
