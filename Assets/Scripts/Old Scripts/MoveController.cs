using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class MoveController : MonoBehaviour
{
    [SerializeField] float moveSpeed, dodgePower, dodgeCD, dodgeSpeed, backstepPower;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform camOrientation, orientation, model;
    [SerializeField] PlayerHealth pHealth;
    [SerializeField] Animator anim;
    [SerializeField] AudioManager audioM;

    Vector3 forward, right;
    Vector3 moveDirection, lastMoveDirection, currentPos, dodgeDestination;

    Quaternion rotDirection;

    public bool canMove, canDodge, canBackstep;
    public bool isMoving, isDodging;
    public float dodgeTime, dodgeDuration, turnSpeed, currentRot;

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

            Vector3 forward = orientation.transform.forward;
            Vector3 right = orientation.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = forward * verticalInput + right * horizontalInput;
            Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);
            Quaternion targetRot = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetRot, turnSpeed);

                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
                canDodge = true;
                canBackstep = false;
            }

            if (Mathf.Abs(horizontalInput) < 0.1f && Mathf.Abs(verticalInput) < 0.1f)
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                canDodge = false;
                canBackstep = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(horizontalInput) < 0.1f && Mathf.Abs(verticalInput) < 0.1f && canBackstep && pHealth.stamina >= 0.2f)
            {
                currentPos = rb.position;
                dodgeDestination = currentPos += -model.forward * backstepPower;
                anim.SetTrigger("backstep");
                dodgeTime = 0;
                StartCoroutine(Backstep(dodgeDestination, .8f));
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

    IEnumerator Backstep(Vector3 dodgeDestination, float duration)
    {
        Vector3 startPosition = transform.position;
        yield return new WaitForSeconds(0.15f);

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