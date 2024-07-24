using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthCount;
    public TextMeshProUGUI manaCount;
    public Image damageEffect;

    public float health, mana;
    public float maxHealth, maxMana;
    public float damageFadeDuration, visibleDamageDuration;

    float previousHealth;

    private void Start()
    {
        health = maxHealth;
        mana = maxMana;
        previousHealth = health;

        if (damageEffect != null)
        {
            Color color = damageEffect.color;
            color.a = 0f;
            damageEffect.color = color;
        }
    }

    private void Update()
    {
        UpdateGUI();

        if (previousHealth > health)
        {
            StartCoroutine(HealthLossEffect());
        }

        previousHealth = health;
    }

    public void UpdateGUI()
    {
        healthCount.text = health.ToString("F0");
        manaCount.text = mana.ToString("F0");
    }

    public IEnumerator HealthLossEffect()
    {
        float elapsedTime = 0f;
        Color color = damageEffect.color;
        while (elapsedTime < damageFadeDuration)
        {
            color.a = Mathf.Lerp(0f, 0.6f, elapsedTime / damageFadeDuration);
            damageEffect.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0.6f;
        damageEffect.color = color;

        yield return new WaitForSeconds(visibleDamageDuration);

        elapsedTime = 0f;
        while (elapsedTime < damageFadeDuration)
        {
            color.a = Mathf.Lerp(0.6f, 0f, elapsedTime / damageFadeDuration);
            damageEffect.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0f;
        damageEffect.color = color;
    }
}
