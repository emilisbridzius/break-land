using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] float moveSpeed, dodgePower, dodgeCD, dodgeSpeed;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform camOrientation, orientation, model;
    [SerializeField] PlayerHealth pHealth;
    [SerializeField] Animator anim;

    Vector3 forward, right;
    Vector3 moveDirection;

    public bool canMove, canDodge;
    public bool isMoving, isRunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
        canDodge = true;
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
            moveDirection = forward * verticalInput + right * horizontalInput;

            // Apply movement to the Rigidbody using MovePosition
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            model.transform.forward = moveDirection;

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                isRunning = true;

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    anim.Play("Run");
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDodge && pHealth.stamina >= 0.3f)
            {
                Dodge();
            }
        }

    }

    void Dodge()
    {
        Vector3 currentPos = rb.position;
        Vector3 dodgeDestination = transform.position += moveDirection * dodgePower;
        Vector3.Lerp(currentPos, dodgeDestination, Time.deltaTime);

        pHealth.stamina -= 0.3f;

        Debug.Log("dodge");
    }

    void Timers()
    {
        if (dodgeCD > 0)
        {
            canDodge = true;
            dodgeCD -= Time.deltaTime;
        }

        if (dodgeCD < 0)
        {
            dodgeCD = 0;
        }
    }
}