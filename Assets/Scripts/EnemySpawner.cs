using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public float[] spawnWeights; // Array of weights for enemy prefabs
    public Transform[] spawnPoints;   // Array of spawn points
    public float baseSpawnInterval = 5f; // Base time interval between spawns
    public float spawnRateIncreaseFactor = 0.9f; // Factor by which spawn rate decreases each round

    private float currentSpawnInterval;
    private GameManager gameManager;
    private bool isSpawning = true;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentSpawnInterval = baseSpawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if (gameManager != null)
        {
            // Adjust spawn interval based on the current round
            currentSpawnInterval = Mathf.Max(0.5f, baseSpawnInterval * Mathf.Pow(spawnRateIncreaseFactor, gameManager.roundCount - 1));
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            if (spawnPoints.Length > 0 && enemyPrefabs.Length > 0)
            {
                SpawnEnemy();
            }

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // Choose a random spawn point without repeating the last one
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Choose a random enemy prefab based on weights
        GameObject enemyPrefab = ChooseEnemyBasedOnWeight();

        // Instantiate a new enemy at the chosen spawn point
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private GameObject ChooseEnemyBasedOnWeight()
    {
        float totalWeight = 0f;
        foreach (float weight in spawnWeights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            cumulativeWeight += spawnWeights[i];
            if (randomValue < cumulativeWeight)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[enemyPrefabs.Length - 1]; // Fallback to last enemy prefab
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopCoroutine(SpawnEnemies());
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }
}