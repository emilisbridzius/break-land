using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RangeDetection : MonoBehaviour
{
    public BoxCollider mCollider;
    public EnemyBehaviour mController;

    public List<GameObject> objectsInRange = new List<GameObject>();
    Coroutine attackChargeUp;

    void Start()
    {
        mCollider = GetComponent<BoxCollider>();
        mController = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider" && !objectsInRange.Contains(other.gameObject))
        {
            objectsInRange.Add(other.gameObject);

            attackChargeUp = StartCoroutine(WaitForAttack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInRange.Remove(other.gameObject);

        if (attackChargeUp != null)
        {
            StopCoroutine(attackChargeUp);
        }
    }

    public IEnumerator WaitForAttack()
    {
        float waitFor = mController.attackWhenInRangeFor;
        float cooldown = mController.attackCooldown;

        yield return new WaitForSeconds(waitFor);

        foreach (GameObject obj in objectsInRange)
        {
            var objectHealth = obj.GetComponentInParent<PlayerHealth>();
            objectHealth.health -= mController.attackDamage;
        }

        mController.Attack();

        yield return new WaitForSeconds(cooldown);
        yield return WaitForAttack();
    }
}
