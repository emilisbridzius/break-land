using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MagicBehaviour : MonoBehaviour
{
    public PlayerAttack master;
    Bounds spawnBounds;

    Vector3 startPos, endPos, midPoint, randomDir, curvePoint;
    float step;
    float boundsLength, boundsWidth;

    void Awake()
    {
        step = 0.0f;
        master = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    void Update()
    {

    }

    #region homing along a curve point 
    public IEnumerator HomingAttack(Transform targetPos)
    {
        CreateCurvePoint();

        while (step < 1.0f)
        {
            transform.position = MoveAlongBezierCurvePoint(step, startPos, curvePoint, endPos);
            step += Time.deltaTime * master.projSpeed;
            yield return null;
        }
        targetPos.GetComponent<EnemyHealth>().health -= master.projDamage;
        Destroy(gameObject);
    }

    void CreateCurvePoint()
    {
        startPos = transform.position;
        endPos = master.targetLock.position;
        midPoint = (startPos + endPos) / 2;

        randomDir = Random.insideUnitCircle;
        float randF = randomDir.y;
        randomDir.y = Mathf.Abs(randF);
        randomDir *= 8;
        curvePoint = randomDir += midPoint;
    }

    Vector3 MoveAlongBezierCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }

    public void HMCR()
    {
        StartCoroutine(HomingAttack(master.targetLock));
    }
    #endregion

    void CreateRandomPillarSpawns()
    {
        spawnBounds = new Bounds(transform.position, new Vector3(boundsWidth, 0, boundsLength));
    }
}
