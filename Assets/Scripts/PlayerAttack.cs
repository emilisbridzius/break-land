using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCD = 0.4f;
    [SerializeField] float knockbackForward, knockbackUp;
    [SerializeField] GameObject player;

    public bool canAttack;
    float combo;
    float maxCombo = 4f;
    float playerNegativeZ;

    List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        GetComponent<Collider>();
        canAttack = true;
    }

    private void Update()
    {
        Timers();

        playerNegativeZ = player.transform.rotation.y;
        knockbackForward = -playerNegativeZ;
        knockbackForward *= 5f;


        if (Input.GetKey(KeyCode.Mouse0) && canAttack || Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            SwingAttack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject);
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void SwingAttack()
    {
        combo++;

        foreach (GameObject enemy in enemiesInRange)
        {
            Rigidbody rb = enemy.GetComponent<Rigidbody>();

            rb.AddForce(0, knockbackUp, knockbackForward, ForceMode.Impulse);
        }

        attackCD = 0.4f;
    }

    void Timers()
    {
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        
        if (attackCD < 0)
        {
            attackCD = 0;
        }

        if (attackCD == 0)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }
}
