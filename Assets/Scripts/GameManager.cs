using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerHealth pHealth;
    [SerializeField, Range(0, 10)] float recoveryRate;

    public bool canRecoverStamina;
    private void Update()
    {
        if (canRecoverStamina && pHealth.stamina < pHealth.maxStamina)
        {
            pHealth.stamina += Time.deltaTime * recoveryRate;
        }
    }
}
