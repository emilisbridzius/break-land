using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

public class MoveController : MonoBehaviour
{
    [SerializeField] float moveSpeed, jumpForce, jumpCD, rayMaxDist, dodgePower, dodgeCD, dodgeSpeed, backstepPower;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform camOrientation, orientation, model, feet;
    [SerializeField] PlayerHealth pHealth;
    [SerializeField] Animator anim;
    AnimationCurve dodgeCurve, backstepCurve;
    [SerializeField] AudioManager audioM;
    [SerializeField] Camera cam;

    Vector3 moveDirection, currentPos, dodgeDestination;
    RaycastHit hitInfo;
    public LayerMask ignoreLayer, groundLayer;

    public bool canMove, canJump, canDodge, canBackstep;
    public bool isMoving, isDodging, isGrounded;
    public float jumpTime, dodgeTime, dodgeDuration, jumpReset, turnSpeed, currentRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
        canDodge = true;
        jumpTime = jumpCD;

        Keyframe dodgeLastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTime = dodgeLastFrame.time;
    }

    private void Update()
    {

        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 totalInput = new Vector2(horizontalInput, verticalInput);
            totalInput = Vector2.ClampMagnitude(totalInput, 1f);

            Vector3 forward = orientation.transform.forward;
            Vector3 right = orientation.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = forward * totalInput.y + right * totalInput.x;
            Quaternion targetRot = Quaternion.LookRotation(moveDirection, Vector3.up);

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRot, turnSpeed);

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

   # region Dodging ifs 
            if (Input.GetKeyDown(KeyCode.LeftShift) && canBackstep && pHealth.stamina >= 0.2f)
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
                anim.SetTrigger("dodge");
                StartCoroutine(Dodge(dodgeDestination, dodgeDuration));
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canJump && isGrounded && jumpTime <= 0)
                {
                    canJump = false;
                    Jump();
                    Invoke(nameof(JumpReset), jumpReset);
                }
            }
            
            GroundCheck();
            StateCheck();
            Timers();

        }

    }

    #region Dodging code
    IEnumerator Dodge(Vector3 dodgeDestination, float duration)
    {
        float timer = 0;
        while (timer < dodgeTime)
        {
            canMove = false;
            float speed = dodgeCurve.Evaluate(timer);
            var grav = Physics.gravity;
            Vector3 dodgeDir = (transform.forward * speed) + grav;
            rb.AddForce(dodgeDir * dodgePower);
            timer += Time.deltaTime;
            yield return null;
        }
        canMove = true; 

        //Vector3 startPosition = transform.position;
        //while (dodgeTime < duration)
        //{
        //    canMove = false;
        //    rb.position = Vector3.Lerp(startPosition, dodgeDestination, dodgeTime / duration);
        //    dodgeTime += Time.deltaTime;
        //    yield return null;
        //}
        //canMove = true;
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
    #endregion

    void Jump() 
    {
        anim.SetTrigger("jump");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(model.up * jumpForce, ForceMode.Impulse);
        rb.AddForce(model.forward * jumpForce / 2, ForceMode.Impulse);
    }

    void JumpReset()
    {
        jumpTime = jumpCD;
        canJump = true;
    }

    public void GroundCheck()
    {
        if (Physics.Raycast(feet.position, Vector3.down, rayMaxDist, groundLayer))
        {
            isGrounded = true;
            canJump = true;
            anim.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            canJump = false;
            anim.SetBool("isGrounded", false);
        }
    }

    public void StateCheck()
    {
        if (isPlaying(anim, "Landing") || isPlaying(anim, "Jump") || isPlaying(anim, "Falling"))
        {
            canJump = false;
        }
    }

    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Landing") && 
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else return false;
    }

    void Timers()
    {
        if (jumpTime > 0)
        {
            jumpTime -= Time.deltaTime;
        }

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