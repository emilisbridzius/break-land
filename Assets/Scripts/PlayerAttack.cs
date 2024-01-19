using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCD, setCD, comboResetAfter;
    [SerializeField] float knockbackForward, knockbackUp;
    [SerializeField] int weaponDamage;
    [SerializeField] GameObject player;
    [SerializeField] TMP_Text comboText;

    public bool canAttack;
    public int combo;
    int maxCombo = 5;
    public float comboResetTimer;
    Vector3 playerOpposite;

    List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        GetComponent<Collider>();
        canAttack = true;

        combo = 0;
        comboResetTimer = 5;
    }

    private void Update()
    {
        Timers();
        UpdateUI();

        playerOpposite = player.transform.forward;

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
        if (enemiesInRange.Count > 0)
        {
            combo++;
            comboResetTimer = comboResetAfter;
        }

        if (combo < maxCombo)
        {
            foreach (GameObject enemy in enemiesInRange)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                rb.AddForce(playerOpposite.x * knockbackForward, knockbackUp, 
                    playerOpposite.z * knockbackForward, ForceMode.Impulse);
                enemyHealth.health -= weaponDamage;
            }
        }
        else if (combo == maxCombo)
        {
            foreach (GameObject enemy in enemiesInRange)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                rb.AddForce(playerOpposite.x * knockbackForward, knockbackUp * 3, 
                    playerOpposite.z * knockbackForward, ForceMode.Impulse);
                enemyHealth.health -= weaponDamage * 2;
            }
            combo = 0;
            comboResetTimer = comboResetAfter;
        }

        attackCD = setCD;
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

        if (combo > 0 && comboResetTimer > 0)
        {
            comboResetTimer -= Time.deltaTime;
        }

        if (comboResetTimer <= 0)
        {
            comboResetTimer = comboResetAfter;
            combo = 0;
        }
    }

    void UpdateUI()
    {
        comboText.text = combo.ToString();
    }
}
