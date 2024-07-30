using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] float maxAcceleration = 100f;
    [SerializeField] float jumpHeight, crouchSpeed, slideForce, wallRunSpeed, wallRunDuration, wallRunGravity;
    public Transform playerInputSpace, feet, head, wallContact;
    public FirstPersonCam camController;
    public CapsuleCollider standingCol;

    public float groundRayMaxDist, ceilingRayMaxDist;
    public LayerMask groundLayer, obstructionLayer;
    public bool canJump, canStand, canWallRun;
    public bool isGrounded, isCrouched, isCamLerping, isSliding, isWallRunning;
    public Vector3 wallNormal;
    public Rigidbody body;

    Vector3 velocity, desiredVelocity;
    Coroutine lerpCam, slideRoutine, wallRunRoutine;
    Ray upRay;

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

        if (Input.GetKey(KeyCode.W) && canWallRun && wallContact != null)
        {
            wallRunRoutine = StartCoroutine(WallRun());
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
        if (!isSliding && !isWallRunning)
        {
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
        canJump = true;
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
        canJump = false;
        standingCol.height = 1;
        standingCol.center = new Vector3(0f, -0.51f, 0f);
    }

    /// <summary>
    /// Manipulate object velocity directly to make the object travel forward until slideDuration is reached.
    /// </summary>
    /// <returns>
    /// IEnumerator returns null until the slide has been completed.
    /// </returns>
    IEnumerator Slide()
    {
        isSliding = true;
        canJump = false;
        float slideDuration = 1.5f;
        float slideTimer = 0f;

        Vector3 slideVelocity = camController.orientation.forward * slideForce;
        body.velocity = new Vector3(slideVelocity.x, body.velocity.y, slideVelocity.z);

        while (slideTimer < slideDuration)
        {
            slideTimer += Time.deltaTime;
            float slideProgress = slideTimer / slideDuration;
            body.velocity = Vector3.Slerp(slideVelocity, Physics.gravity, slideProgress);
            yield return null;
        }
        isSliding = false;
        canJump = true;
    }

    IEnumerator WallRun()
    {
        isWallRunning = true;
        canJump = true;
        float wallRunDuration = 1.5f;
        float wallRunTimer = 0f;

        Vector3 tempGrav = Physics.gravity / 10;

        Vector3 wallRunVelocity = wallContact.right * wallRunSpeed;
        body.velocity = new Vector3(wallRunVelocity.x, body.velocity.y, wallRunVelocity.z);

        while (wallRunTimer < wallRunDuration)
        {
            wallRunTimer += Time.deltaTime;
            float wallRunProgress = wallRunTimer / wallRunDuration;
            body.velocity = Vector3.Slerp(wallRunVelocity, tempGrav, wallRunProgress);
            body.mass = 0.001f;
            yield return null;
        }
        isWallRunning = false;
    }

    /// <summary>
    /// Manipulate camera position directly to make it smoothly interpolate to the targetPos with a given duration.
    /// </summary>
    /// <param name="targetPos">The desired position of the camera.</param>
    /// <param name="duration">The total duration for which the camera will take to move to the desired position.</param>
    /// <returns>
    /// IEnumerator returns null until the camera reaches the targetPos.
    /// </returns>
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

    /// <summary>
    /// Fire a raycast from a given position to check if the position is close enough to an object layer.
    /// </summary>
    void GroundedCheck()
    {
        if (Physics.Raycast(feet.position, Vector3.down, groundRayMaxDist, groundLayer))
        {
            isGrounded = true;
            canWallRun = false;

            if (!isSliding)
            {
                canJump = true;
            }
        }
        else
        {
            isGrounded = false;
            canJump = false;
            canWallRun = true;
        }
    }

    void CeilingCheck()
    {
        upRay = new Ray(head.transform.position, Vector3.up);

        if (Physics.SphereCast(upRay, 0.5f, ceilingRayMaxDist, obstructionLayer))
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
