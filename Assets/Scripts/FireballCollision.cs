using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    public PlayerAttack attack;
    public GameObject explosionEffect;

    // Start is called before the first frame update
    void Awake()
    {
        attack = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerAttack>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wing"))
        {
            var wingHealth = collision.collider.GetComponent<WingsHealth>();
            wingHealth.health -= attack.projDamage;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Enemy") && collision.collider.GetComponent<EnemyHealth>().canBeDamaged)
        {
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.health -= attack.projDamage;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
