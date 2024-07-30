using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class RangedEnemyBehaviour : MonoBehaviour
{
    [Header("FOV Settings")]
    public float radius;
    [Range(0, 360)]
    public float angle;

    [Header("GameObj + Transform Ref")]
    public GameObject playerRef;
    public Transform playerPos;
    public Transform enemyAgentTransform, modelForward;
    public Rigidbody enemyRigidbody;

    [Header("NavMesh + LayerMask Settings")]
    public NavMeshAgent enemyAgent;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Attack Settings")]
    public GameObject attackProjectile;
    public float attackCooldown;
    public int attackDamage;

    [Header("Debugging Tools for FOV")]
    public bool canSeePlayer;
    public Animator anim;

    private void Start()
    {
        playerRef = GameObject.Find("Player");
        playerPos = playerRef.gameObject.transform;

        enemyAgentTransform = transform;
        enemyRigidbody = GetComponent<Rigidbody>();

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
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }

        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }


    public void Update()
    {
        if (!canSeePlayer)
        {
            canSeePlayer = true;
        }

        if (canSeePlayer && Vector3.Distance(enemyAgent.transform.position, playerPos.position) > 2.3f)
        {
            enemyAgent.isStopped = false;
            enemyAgent.SetDestination(playerPos.position);
            Running();
        }
        else
        {
            enemyAgent.isStopped = true;
            Idle();
        }

        SetVerticalPosition();
    }

    public void SetVerticalPosition()
    {
        Vector3 currentHeight = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        currentHeight.y += 10f;
    }

    void Idle()
    {
        anim.SetBool("isMoving", false);
    }

    void Running()
    {
        anim.SetBool("isMoving", true);
    }

    public void Attack()
    {
        anim.SetTrigger("doAttack");

    }

}