using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MagicBehaviour : MonoBehaviour
{
    public PlayerAttack master;
    public GameObject spike;
    public Transform groundSpawn;
    Bounds spawnBounds;
    public int spikesToSpawn = 10;
    public float boundsLength, boundsWidth, fireProjCurveSpeed;
    List<Transform> enemies = new List<Transform>();

    Vector3 startPos, endPos, midPoint, randomDir, curvePoint;
    float step;

    void Awake()
    {
        step = 0.0f;
        master = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerAttack>();
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
            step += Time.deltaTime * fireProjCurveSpeed;
            yield return null;
        }
    }

    void CreateCurvePoint()
    {
        startPos = master.spawnPos.position;
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

    #region creating spikes with random positions, rotations and scale within a bound
    public void CreateSpawnBounds()
    {
        spawnBounds = new Bounds(groundSpawn.position, new Vector3(boundsWidth, master.transform.position.y, boundsLength));

        SpawnPillars();
    }

    Vector3 GetRandomPositionInBounds()
    {
        float randomX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        float randomY = Random.Range(spawnBounds.min.z, spawnBounds.max.z);
        Vector3 randomPos = new Vector3(randomX, spawnBounds.center.y, randomY);

        return randomPos;
    }

    Quaternion GetRandomRotationInBounds()
    {
        float randomZ = Random.Range(30, -50);
        float randomX = Random.Range(0, 50);
        float randomY = Random.Range(30, 50);
        Quaternion randomRot = new Quaternion(randomX, randomY, randomZ, 0);

        return randomRot;
    }

    Vector3 GetRandomScaleInBounds()
    {
        float randomY = Random.Range(4, 5.5f);
        Vector3 randomScale = new Vector3(0.55f, 0.55f, randomY);

        return randomScale;
    }

    void SpawnPillars()
    {
        for (int i = 0; i < spikesToSpawn; i++)
        {
            Vector3 randomPosition = GetRandomPositionInBounds();
            Quaternion randomRotation = GetRandomRotationInBounds();
            GameObject instSpike = Instantiate(spike, randomPosition, randomRotation);
            instSpike.transform.localScale = GetRandomScaleInBounds();
        }
    }
    #endregion

    #region creating a slab that bounces between enemies
    void SpawnSlab()
    {

    }
    #endregion
}
