using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossBehaviour : MonoBehaviour
{
    [Header("FOV Settings")]
    public float radius;
    [Range(0, 360)]
    public float angle;
    public bool chasePlayer;

    [Header("GameObj + Transform Ref")]
    public GameObject playerRef;
    public Vector3 playerPos;
    public Transform enemyAgentTransform;

    [Header("NavMesh + LayerMask Settings")]
    public NavMeshAgent enemyAgent;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Debugging Tools for FOV")]
    public bool canSeePlayer;
    public bool inAttackRange;

    public Animator anim;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerRef.transform.position;
        enemyAgentTransform = transform;
        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
            }
        }
    }

    public void Update()
    {
        playerPos = playerRef.transform.position;

        if (canSeePlayer)
        {
            anim.SetBool("isMoving", true);
            MoveAndFacePos(playerPos);
        }
        else if (!canSeePlayer)
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void MoveAndFacePos(Vector3 toPos)
    {
        toPos.y = enemyAgentTransform.position.y;
        enemyAgentTransform.LookAt(toPos);
        enemyAgent.SetDestination(toPos);
    }
}