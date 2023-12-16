using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveMult;
    [SerializeField] float jumpMult;
    [SerializeField] Rigidbody body;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveMult * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveMult * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveMult * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * moveMult * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(transform.up * jumpMult, ForceMode.VelocityChange);
        }
    }
}
