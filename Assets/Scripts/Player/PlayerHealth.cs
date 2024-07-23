using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthCount;
    public TextMeshProUGUI manaCount;

    public float health, mana;
    public float maxHealth, maxMana;

    private void Start()
    {
        health = maxHealth;
        mana = maxMana;
    }

    private void Update()
    {
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        healthCount.text = health.ToString("F0");
        manaCount.text = mana.ToString("F0");
    }
}
