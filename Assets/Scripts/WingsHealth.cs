using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsHealth : MonoBehaviour
{
    public float health, maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
