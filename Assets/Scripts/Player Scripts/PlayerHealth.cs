using ECM.Components;
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
    Menus menus;

    [FMODUnity.EventRef]
    public string PlayerHurtEvent = "";

    [FMODUnity.EventRef]
    public string PlayerDeathEvent = "";



    private void Awake()
    {
        playerController = GetComponent<PlayerMoonGolfController>();
        healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        animate = GetComponent<PlayerAnimations>();
        menus = GameObject.Find("LevelDetailsCanvas").GetComponent<Menus>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
            FMODUnity.RuntimeManager.PlayOneShot(PlayerDeathEvent, transform.position);
        }
        else
        {
            animate.Damaged(amount);
            FMODUnity.RuntimeManager.PlayOneShot(PlayerHurtEvent, transform.position);
        }
    }

    public void Death()
    {
        isDead = true;
        animate.Death();
        playerController.moveDirection = Vector3.zero;
        playerController.isDead = true;
        menus.TurnOnDeathMenu();
    }
}
