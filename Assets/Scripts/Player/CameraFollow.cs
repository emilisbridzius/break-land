using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera cam;
    public Movement move;
    public FirstPersonCam fps;

    private void Update()
    {
        if (move.isCrouched)
        {
            cam.transform.position = fps.crouchedCamPos.position;
        }

        if (!move.isCrouched)
        {
            cam.transform.position = fps.camPos.position;
        }
    }
}
