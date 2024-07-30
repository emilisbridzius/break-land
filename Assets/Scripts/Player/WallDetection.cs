using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public Movement move;
    public FirstPersonCam cam;

    private void Start()
    {
        move.body = GetComponent<Rigidbody>(); // Get the Rigidbody component
        if (move.body == null)
        {
            Debug.LogError("No Rigidbody found on this GameObject.");
        }
    }

    private void Update()
    {
        LedgeSweep();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is significant enough to consider it a wall contact
        if (IsWall(collision))
        {
            move.wallContact = collision.transform; // Store the wall object
            move.canWallRun = true;
            move.wallTangent = Vector3.ProjectOnPlane(cam.transform.forward, move.contactToTangent);
            Debug.Log("Contact with wall detected: " + move.wallContact.name);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Continue checking for wall contact
        if (IsWall(collision))
        {
            move.wallContact = collision.transform; // Update the wall object
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsWall(collision))
        {
            move.wallContact = null; // Update the wall object
            move.canWallRun = false;
        }
    }

    private bool IsWall(Collision collision)
    {
        // Define the logic to determine if the collision is with a wall
        // For simplicity, we consider any contact that isn't too small or too large
        // as a wall contact.

        foreach (ContactPoint contact in collision.contacts)
        {
            // Example: Consider it a wall if the contact point's normal is roughly vertical
            if (Mathf.Abs(Vector3.Dot(contact.normal, Vector3.up)) < 0.1f)
            {
                move.contactToTangent = contact.normal;
                return true; // This means the contact is more horizontal, indicating a wall
            }
        }

        return false;
    }

    void LedgeSweep()
    {
        
    }

    private bool isLedge()
    {
        return false;
    }
}
