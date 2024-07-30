using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsHealth : MonoBehaviour
{
    public float health, maxHealth;
    public EnemyHealth enemyHealth;

    private void Start()
    {
        health = maxHealth;
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void Update()
    {
        if (health <= 0) 
        {
            enemyHealth.wings.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
