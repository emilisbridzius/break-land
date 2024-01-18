using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCam : MonoBehaviour
{
    [SerializeField] float xSensitivity, ySensitivity, bobSpeed, bobAmount;
    [SerializeField] Transform orientation, cameraPos;

    float xRotation, yRotation, defaultPosY;
    float timer = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultPosY = transform.localPosition.y;
    }

    private void Update()
    {
        // mouse input
        float mouseX = (Input.GetAxisRaw("Mouse X") * xSensitivity);
        float mouseY = (Input.GetAxisRaw("Mouse Y") * ySensitivity);

        yRotation += mouseX;
        xRotation -= mouseY;

        // prevent breaking your neck
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    //void Bobbing()
    //{
    //    float horizontalInput = Input.GetAxis("Horizontal");
    //    float verticalInput = Input.GetAxis("Vertical");

    //    if (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(horizontalInput) > 0.1f)
    //    {
    //        // Player is moving
    //        timer += Time.deltaTime * bobSpeed;
    //        float newY = defaultPosY + Mathf.Sin(timer) * bobAmount;
    //        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    //    }
    //    else
    //    {
    //        // Idle
    //        timer = 0;
    //        float newY = Mathf.Lerp(transform.localPosition.y, defaultPosY, bobSpeed);
    //        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    //    }
    //}
}