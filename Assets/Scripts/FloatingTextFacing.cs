using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextFacing : MonoBehaviour
{
    [SerializeField] GameObject playerCam;

    private void Start()
    {
        playerCam = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        Quaternion look = Quaternion.LookRotation(playerCam.transform.forward, Vector3.up);
        transform.rotation = look;
    }
}
