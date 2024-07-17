using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera cam;
    public Movement move;
    public FirstPersonCam fps;

    public float smoothingFactor;

    private void LateUpdate()
    {
        if (move.isCamLerping)
        {
            // If camera is lerping, still follow the player smoothly
            Vector3 targetPos = move.isCrouched ? fps.crouchedCamPos.position : fps.camPos.position;
            cam.transform.position = Vector3.Slerp(cam.transform.position, targetPos, Time.deltaTime * smoothingFactor); // Adjust the smoothing factor as needed
        }
        else
        {
            // Directly follow the player's position
            Vector3 targetPos = move.isCrouched ? fps.crouchedCamPos.position : fps.camPos.position;
            cam.transform.position = targetPos;
        }
    }
}
