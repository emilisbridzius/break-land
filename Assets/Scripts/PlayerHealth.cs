using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float stamina;
    public float maxStamina;
    [SerializeField] Image healthBar, staminaBar;

    private void Start()
    {
        health = maxHealth;
        stamina = maxStamina;
    }

    private void Update()
    {
        healthBar.fillAmount = health;
        staminaBar.fillAmount = stamina;
    }
}
