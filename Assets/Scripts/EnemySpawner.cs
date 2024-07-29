using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // List of spawn positions
    public List<Transform> spawnPositions = new List<Transform>();

    // List of enemy prefabs to spawn
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    // Initial interval between spawns in seconds
    public float spawnInterval;

    // Multiplier increment every 30 seconds
    public float multiplierIncrement;

    private float currentMultiplier = 1f;
    private float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseMultiplier());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval / currentMultiplier);

            // Select a random spawn position
            Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

            // Select a random enemy prefab
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // Spawn the enemy
            Instantiate(enemyPrefab, spawnPosition.position, spawnPosition.rotation);
        }
    }

    IEnumerator IncreaseMultiplier()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            // Increase the multiplier
            currentMultiplier += multiplierIncrement;
            Debug.Log("Multiplier increased to: " + currentMultiplier);
        }
    }
}
