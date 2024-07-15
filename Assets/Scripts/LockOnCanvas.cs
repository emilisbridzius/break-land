using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnCanvas : MonoBehaviour
{
    public Camera cam;
    public Image targetIndicator;
    public PlayerAttack attack;

    float minScale = 0.5f;
    float maxScale = 1.0f;
    float maxDistance = 10f;

    void Start()
    {
        targetIndicator.enabled = false;
    }

    void Update()
    {
        if (attack.targetLock.GetChild(1).GetComponent<Renderer>().isVisible)
        {
            targetIndicator.enabled = true;
            UpdateIndicatorPosition();
            print("enemy visible");
        }
        else { targetIndicator.enabled = false; }
    }

    void UpdateIndicatorPosition()
    {
        Vector2 camPos = cam.WorldToScreenPoint(attack.targetLock.Find("TargetIndicatorPos").position);
        float dist = Vector3.Distance(cam.transform.position, attack.targetLock.position);
        float scaleFactor = Mathf.Lerp(maxScale, minScale, dist / maxDistance);
        scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);

        targetIndicator.transform.localScale = new Vector2(scaleFactor, scaleFactor);
        targetIndicator.transform.position = camPos;
    }
}
