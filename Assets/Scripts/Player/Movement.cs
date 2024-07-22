using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] float maxAcceleration = 100f;
    [SerializeField] float jumpHeight, crouchSpeed, slideForce;
    [SerializeField] Transform playerInputSpace, feet, head;
    public FirstPersonCam camController;
    public CapsuleCollider standingCol;

    public float groundRayMaxDist, ceilingRayMaxDist;
    public LayerMask groundLayer, obstructionLayer;
    public bool canJump, canStand;
    public bool isGrounded, isCrouched, isCamLerping, isSliding;

    Vector3 velocity, desiredVelocity, upAxis;
    Rigidbody body;
    RaycastHit abovePlayer;
    Coroutine lerpCam, slideRoutine;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        standingCol.enabled = true;
    }

    void Update()
    {
        GroundedCheck();
        CeilingCheck();

        Vector2 playerInput;

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (playerInputSpace)
        {
            Vector3 forward = playerInputSpace.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = playerInputSpace.right;
            right.y = 0f;
            right.Normalize();

            desiredVelocity = (forward * playerInput.y + right * playerInput.x) * maxSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded && body.velocity.magnitude > 0.7f)
            {
                if (!isSliding && !isCrouched)
                {
                    slideRoutine = StartCoroutine(Slide());
                }
                Crouched();
            }
            else if (isGrounded)
            {
                Crouched();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isGrounded && canStand)
        {
            Standing();
        }
    }

    private void FixedUpdate()
    {
        if (!isSliding)
        {
            upAxis = -Physics.gravity.normalized;

            velocity = body.velocity;
            float maxSpeedChange = maxAcceleration * Time.deltaTime;

            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            body.velocity = velocity;
        }
    }

    void Jump()
    {
        velocity.y += Mathf.Sqrt(2 * jumpHeight * (-Physics.gravity.y * 0.5f));
        body.velocity = velocity;
    }

    public void Standing()
    {
        if (lerpCam != null)
        {
            StopCoroutine(lerpCam);
        }

        if (slideRoutine != null)
        {
            StopCoroutine(slideRoutine);
            isSliding = false;
        }

        lerpCam = StartCoroutine(LerpCamera(camController.camPos, crouchSpeed));
        maxSpeed = 10f;

        isCrouched = false;
        isSliding = false;

        standingCol.height = 2;
        standingCol.center = new Vector3(0f, 0f, 0f);
    }

    public void Crouched()
    {
        if (lerpCam != null)
        {
            StopCoroutine(lerpCam);
        }

        lerpCam = StartCoroutine(LerpCamera(camController.crouchedCamPos, crouchSpeed));
        maxSpeed = 5f;

        isCrouched = true;
        standingCol.height = 1;
        standingCol.center = new Vector3(0f, -0.51f, 0f);
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        float slideDuration = 1.5f;
        float slideTimer = 0f;

        Vector3 slideVelocity = camController.orientation.forward * slideForce;
        body.velocity = new Vector3(slideVelocity.x, body.velocity.y, slideVelocity.z);

        while (slideTimer < slideDuration)
        {
            slideTimer += Time.deltaTime;
            float slideProgress = slideTimer / slideDuration;
            body.velocity = Vector3.Lerp(slideVelocity, Vector3.zero, slideProgress);
            body.AddForce(Vector3.down * 10f);
            yield return null;
        }
    }

    IEnumerator LerpCamera(Transform targetPos, float duration)
    {
        isCamLerping = true;
        float time = 0;
        Vector3 initialPos = camController.cam.transform.position;

        while (time < duration)
        {
            camController.cam.transform.position = Vector3.Slerp(initialPos, targetPos.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        camController.cam.transform.position = targetPos.position;
        isCamLerping = false;
    }

    void GroundedCheck()
    {
        if (Physics.Raycast(feet.position, Vector3.down, groundRayMaxDist, groundLayer))
        {
            isGrounded = true;
            canJump = true;
        }
        else
        {
            isGrounded = false;
            canJump = false;
        }
    }

    void CeilingCheck()
    {
        if (Physics.SphereCast(head.transform.position, 0.5f, Vector3.up, out abovePlayer, ceilingRayMaxDist, obstructionLayer))
        {
            canStand = false;
        }
        else
        {
            canStand = true;

            if (!Input.GetKey(KeyCode.LeftShift) && isCrouched)
            {
                Standing();
            }
        }
    }
}
