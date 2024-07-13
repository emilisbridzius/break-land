using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;

    [SerializeField] TMP_Text healthBar;
    RagdollController ragdoll;

    private void Start()
    {
        ragdoll = GetComponent<RagdollController>();
    }

    private void Update()
    {
        //UpdateUI();
        
        if (health <= 0 )
        {
            ragdoll.Die();
        }
    }

    void UpdateUI()
    {
        healthBar.text = health.ToString();
    }
}
