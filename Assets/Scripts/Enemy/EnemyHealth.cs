using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    RagdollController ragdoll;
    PlayerTimer timer;

    public int health;
    public int maxHealth;

    public bool hasDied;

    private void Start()
    {
        ragdoll = GetComponent<RagdollController>();
        timer = FindObjectOfType<PlayerTimer>();
        health = maxHealth;
        hasDied = false;
    }

    private void Update()
    {        
        if (health <= 0 && !hasDied)
        {
            ragdoll.Die();
            hasDied = true;

            timer.time += 2f;
        }
    }
}
