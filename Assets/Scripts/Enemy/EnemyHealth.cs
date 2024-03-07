using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;

    [SerializeField] TMP_Text healthBar;

    private void Update()
    {
        UpdateUI();
        

    }

    void UpdateUI()
    {
        healthBar.text = health.ToString();
    }
}
