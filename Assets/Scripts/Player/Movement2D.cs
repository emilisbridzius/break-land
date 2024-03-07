using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement2D : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] float maxAcceleration = 10f;
    [SerializeField, Range(0f, 100f)] float jumpHeight = 5f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform playerInputSpace;

    Vector2 playerInput;
    Vector3 desiredVelocity, velocity;
    Rigidbody body;
    public bool onGround;
    float activeJumps;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GroundCheck();

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (playerInputSpace)
        {
            Vector3 right = playerInputSpace.right;
            right.y = 0f;
            right.Normalize();

            desiredVelocity = (right * playerInput.x) * maxSpeed;
        }

        if (onGround && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (onGround && activeJumps > 0)
        {
            activeJumps = 0;
        }
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    void Jump()
    {
        velocity.y += jumpHeight;
        activeJumps += 1;
        Debug.Log("jumped");
    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }
}
