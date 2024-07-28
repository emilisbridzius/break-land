using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnCanvas : MonoBehaviour
{
    public Camera cam;
    public Image targetIndicator;
    public PlayerAttack attack;

    float minScale = 0.05f;
    float maxScale = 1.0f;
    public float maxDistance = 10f;
    public float indicatorRotSpeed;

    void Start()
    {
        targetIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (attack.targetLock.GetComponentInParent<EnemyHealth>().health > 0)
        {
            targetIndicator.gameObject.SetActive(true);
            UpdateIndicatorPosition();
        }
        else
        {
            targetIndicator.gameObject.SetActive(false);
        }
    }

    void UpdateIndicatorPosition()
    {
        Vector2 camPos = cam.WorldToScreenPoint(attack.targetLock.Find("TargetIndicatorPos").position);
        float dist = Vector3.Distance(cam.transform.position, attack.targetLock.position);
        float scaleFactor = Mathf.Lerp(maxScale, minScale, dist / maxDistance);
        scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);

        targetIndicator.transform.localScale = new Vector2(scaleFactor, scaleFactor);
        targetIndicator.transform.position = camPos;

        if (targetIndicator != null)
        {
            targetIndicator.rectTransform.Rotate(Vector3.forward, indicatorRotSpeed * Time.deltaTime);
        }
    }
}
