using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public List<GameObject> fireballs = new List<GameObject>();
    public Transform spawnPos, chargePos, targetLock;
    public GameObject fireball;
    public Camera cam;
    public RaycastHit hit;

    public float projSpeed, timeUntilCharge;
    public int projDamage, maxCharge;

    private float heldFor, chargeTime;

    private void Start()
    {
        chargeTime = 0f;
    }

    private void Update()
    {
        TargetCheck();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            heldFor += Time.deltaTime;
            chargeTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (heldFor > 0.2f && fireballs.Count > 0)
            {
                chargeTime = 0f;
                foreach (GameObject fb in fireballs)
                {
                    fb.GetComponent<FireballBehaviour>().HMCR();
                }
                fireballs.Clear();
            }
            else
            {
                CreateFireball();
            }
            heldFor = 0;
        }

        if (chargeTime >= timeUntilCharge && fireballs.Count < maxCharge)
        {
            ChargeFireball();
            chargeTime = 0.0f;
        }

        if (fireballs.Count > 0)
        {
            SpinFireballsInPlace();
        }
    }

    void TargetCheck()
    {
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity);

        if (hit.collider != null && hit.collider.transform.CompareTag("Enemy"))
        {
            targetLock = hit.transform;
        }
    }

    void CreateFireball()
    {
        GameObject frb = Instantiate(fireball, spawnPos.position, cam.transform.rotation);
        frb.GetComponent<FireballBehaviour>().HMCR();
    }

    void ChargeFireball()
    {
        GameObject frb = Instantiate(fireball, chargePos.position, cam.transform.rotation);
        fireballs.Add(frb);
    }

    void SpinFireballsInPlace()
    {
        foreach (GameObject fb in fireballs)
        {
            fb.transform.RotateAround(chargePos.position, chargePos.up, Random.Range(0, 360));
        }
    }
}
