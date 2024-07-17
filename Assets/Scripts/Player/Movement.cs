using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] float maxAcceleration = 10f;
    [SerializeField] float jumpHeight, crouchSpeed;
    [SerializeField] Transform playerInputSpace, feet, head;
    public FirstPersonCam camController;
    public Collider standingCol, crouchedCol;

    public float groundRayMaxDist, ceilingRayMaxDist;
    public LayerMask groundLayer, obstructionLayer;
    public bool canJump, canStand;
    public bool isGrounded, isCrouched, isCamLerping;

    Vector3 velocity, desiredVelocity, upAxis;
    Rigidbody body;
    RaycastHit abovePlayer;
    Coroutine lerpCam;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        standingCol.enabled = true;
        crouchedCol.enabled = false;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                Jump();
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded)
            {
                Crouched();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (isGrounded && canStand)
            {
                Standing();
            }
            else
            {
                Crouched();
            }
        }

    }

    private void FixedUpdate()
    {
        upAxis = -Physics.gravity.normalized;

        velocity = body.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        body.velocity = velocity;
    }

    void Jump()
    {
        velocity.y += 5;
        body.velocity = velocity;
    }

    public void Standing()
    {
        if (lerpCam != null)
        {
            StopCoroutine(lerpCam);
        }

        lerpCam = StartCoroutine(LerpCamera(camController.camPos, crouchSpeed));
        maxSpeed = 10f;

        isCrouched = false;
        standingCol.enabled = true;
        crouchedCol.enabled = false;
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
        standingCol.enabled = false;
        crouchedCol.enabled = true;
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

        //Debug.DrawRay(head.transform.position, Vector3.up * 1f, Color.red);
    }
}
