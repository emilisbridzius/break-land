using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagic : MonoBehaviour
{
    public GameObject fireball;
    public Rigidbody rb;

    public PlayerAttack attack;
    public MoveController move;
    public MagicBehaviour magic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tier1Fire()
    {
        GameObject fire = Instantiate(fireball, attack.spawnPos.position, Quaternion.identity);
        rb = fire.GetComponent<Rigidbody>();
        
        if (attack.targetLock != null)
        {
            rb.AddForce(attack.normalizedDirection * attack.projSpeed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(attack.spawnPos.transform.forward * attack.projSpeed, ForceMode.Impulse);
        }
    }

    public void Tier2Fire()
    {
        magic.HMCR();
    }
}
