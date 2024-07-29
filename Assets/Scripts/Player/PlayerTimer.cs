using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    public GameManager manager;
    public TextMeshProUGUI timeCounter;
    public Image damageEffect;

    public float time;
    public float maxTime;
    public float damageFadeDuration, visibleDamageDuration;

    private void Start()
    {
        time = maxTime;

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
        TimeCheck();
    }

    public void UpdateGUI()
    {
        timeCounter.text = time.ToString("F0");
    }

    void TimeCheck()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }

        if (time <= 0)
        {
            manager.EndGame();
        }
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
