using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] float moveSpeed, dashPower;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform camOrientation;
    [SerializeField] PlayerHealth pHealth;

    Vector3 forward, right;

    public bool canMove;
    public bool isMoving;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            // Get input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate movement direction relative to camera
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            // Ensure vertical movement is only along the horizontal plane
            forward.y = 0f;
            right.y = 0f;

            // Normalize vectors to ensure consistent speed in all directions
            forward.Normalize();
            right.Normalize();

            // Calculate movement direction based on input and camera orientation
            Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

            // Apply movement to the Rigidbody using MovePosition
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift) && pHealth.dash >= 1)
            {
                rb.AddForce(moveDirection.x * dashPower, 0, moveDirection.z * dashPower, ForceMode.VelocityChange);
                Debug.Log("dash");
            }
        }

    }
}