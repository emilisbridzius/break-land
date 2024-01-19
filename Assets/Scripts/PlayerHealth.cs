using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    public float dash, dashCD;
    [SerializeField] Image healthBar, dashIcon;

    private void Start()
    {
        health = maxHealth;
        dash = 1;
    }

    private void Update()
    {
        DashTimer();
        healthBar.fillAmount = health;
        dashIcon.fillAmount = dash;
    }

    void DashTimer()
    {
        if (dash < 1)
        {
            dash += (dashCD * Time.deltaTime);
        }


    }
}
