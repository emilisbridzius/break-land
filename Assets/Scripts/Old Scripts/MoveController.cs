using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] float moveSpeed, dodgePower, dodgeCD, dodgeSpeed, jumpPowerXZ, jumpPowerY;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform camOrientation, orientation, model;
    [SerializeField] PlayerHealth pHealth;
    [SerializeField] Animator anim;

    Vector3 forward, right;
    Vector3 moveDirection, lastMoveDirection, currentPos, dodgeDestination, jumpDestination;

    public bool canMove, canDodge, canJump;
    public bool isMoving, isDodging;
    public float dodgeTime, dodgeDuration, jumpTime, jumpDuration;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
        canDodge = true;
        canJump = true;
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

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                model.transform.forward = moveDirection;

                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
                canDodge = true;
                canJump = true;
            }

            if (Mathf.Abs(horizontalInput) < 0.1f && Mathf.Abs(verticalInput) < 0.1f)
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                canDodge = false;
                canJump = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDodge && pHealth.stamina >= 0.3f)
            {
                currentPos = rb.position;
                dodgeDestination = currentPos += model.forward * dodgePower;
                anim.SetTrigger("dodge");
                dodgeTime = 0;
                StartCoroutine(Dodge(dodgeDestination, dodgeDuration));
            }

            if (Input.GetKeyDown(KeyCode.Space) && canJump && pHealth.stamina >= 0.4f)
            {
                currentPos = rb.position;
                jumpDestination = currentPos += model.forward * jumpPowerXZ;
                anim.SetTrigger("jump");
                jumpTime = 0;
                StartCoroutine(JumpRoll(jumpDestination, jumpDuration));
            }
        }

    }

    IEnumerator Dodge(Vector3 dodgeDestination, float duration)
    {
        Vector3 startPosition = transform.position;
        while (dodgeTime < duration)
        {
            canMove = false;
            transform.position = Vector3.Lerp(startPosition, dodgeDestination, dodgeTime / duration);
            dodgeTime += Time.deltaTime;
            yield return null;
        }
        canMove = true;
    }

    IEnumerator JumpRoll(Vector3 jumpDestination, float duration)
    {
        Vector3 startPosition = transform.position;
        while (jumpTime < duration)
        {
            canMove = false;
            transform.position = Vector3.Lerp(startPosition, jumpDestination, jumpTime / duration);
            jumpTime += Time.deltaTime;
            yield return null;
        }
        canMove = true;
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