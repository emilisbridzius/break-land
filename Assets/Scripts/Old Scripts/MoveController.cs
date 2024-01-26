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
    Vector3 moveDirection, lastMoveDirection, currentPos, dodgeDestination, moveDir;

    public bool canMove, canDodge;
    public bool isMoving, isDodging;
    public float dodgeTime, dodgeDuration;

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
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = forward * verticalInput + right * horizontalInput;

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                model.transform.forward = moveDirection;

                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
                canDodge = true;
            }

            if (Mathf.Abs(horizontalInput) < 0.1f && Mathf.Abs(verticalInput) < 0.1f)
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                canDodge = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDodge && pHealth.stamina >= 0.3f)
            {
                currentPos = rb.position;
                dodgeDestination = currentPos += model.forward * dodgePower;
                anim.SetTrigger("dodge");
                dodgeTime = 0;
                StartCoroutine(Dodge(dodgeDestination, dodgeDuration));
            }

        }

    }

    IEnumerator Dodge(Vector3 dodgeDestination, float duration)
    {
        Vector3 startPosition = transform.position;
        while (dodgeTime < duration)
        {
            canMove = false;
            rb.position = Vector3.Lerp(startPosition, dodgeDestination, dodgeTime / duration);
            dodgeTime += Time.deltaTime;
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