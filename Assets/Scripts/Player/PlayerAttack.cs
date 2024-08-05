using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public List<Animation> magicAnims = new List<Animation>();
    public Transform targetLock, spawnPos, groundSpawnPos;
    public Camera cam;
    public RaycastHit hit;

    public FireMagic fireMagic;

    public float projSpeed;
    public int projDamage;
    public bool fireMage, airMage, waterMage, earthMage;

    public Vector3 normalizedDirection;

    private void Start()
    {
        
    }

    private void Update()
    {
        //TargetCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void TargetCheck()
    {
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity);

        if (hit.collider != null && hit.collider.GetComponentInParent<EnemyHealth>().health > 0)
        {
            targetLock = hit.transform.GetComponentInParent<Transform>();
            GetTargetPosition();
        }
        else
        {
            targetLock = null;
        }
    }

    public void GetTargetPosition()
    {
        Vector3 spawnPosition = spawnPos.position;
        Vector3 targetPosition = targetLock.position;

        Vector3 direction = targetPosition - spawnPosition;
        direction.y = 0f;

        normalizedDirection = direction;
    }

    

    void Attack()
    {
        if (fireMage)
        {
            fireMagic.Tier1Fire();
            //fireMagic.Tier2Fire();
        }
    }
}
