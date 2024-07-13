using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour
{

    private void Awake()
    {
        setRigidbodyState(true);
        setColliderState(false);
        GetComponent<Animator>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
    }

    void Update()
    {
        
    }

    public void Die()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
    }

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }
}
