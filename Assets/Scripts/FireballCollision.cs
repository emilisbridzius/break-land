using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    public PlayerAttack attack;

    // Start is called before the first frame update
    void Awake()
    {
        attack = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerAttack>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.health -= attack.projDamage; 
            Destroy(gameObject);
        }
    }
}
