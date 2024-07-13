using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    public PlayerAttack attack;

    // Start is called before the first frame update
    void Start()
    {
        attack  = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
