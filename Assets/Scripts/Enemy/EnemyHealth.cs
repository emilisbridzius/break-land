using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    RagdollController ragdoll;
    PlayerTimer timer;
    public List<GameObject> wings = new List<GameObject>();

    public int health;
    public int maxHealth;
    public float timeGain;

    public bool hasDied, canBeDamaged;

    private void Start()
    {
        ragdoll = GetComponent<RagdollController>();
        timer = FindObjectOfType<PlayerTimer>();
        health = maxHealth;
        hasDied = false;
    }

    private void Update()
    {    
        if (wings.Count == 0)
        {
            canBeDamaged = true;
        }
        else
        {
            canBeDamaged = false;
        }

        if (health <= 0 && !hasDied)
        {
            ragdoll.Die();
            hasDied = true;

            timer.time += timeGain;
        }
    }
}
