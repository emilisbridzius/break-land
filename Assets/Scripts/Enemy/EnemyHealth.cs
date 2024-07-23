using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    RagdollController ragdoll;

    public int health;
    public int maxHealth;

    private void Start()
    {
        ragdoll = GetComponent<RagdollController>();
        health = maxHealth;
    }

    private void Update()
    {        
        if (health <= 0 )
        {
            ragdoll.Die();
        }
    }
}
