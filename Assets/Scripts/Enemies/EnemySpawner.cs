using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;

    public List<Wave> waves; 

    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private float nextSpawnTime = 0f;
    private bool waitingForWaveEnd = false;

    [System.Serializable]
    public class Wave
    {
        public int numEnemies;
        public float spawnDelay;
    }

    void Start()
    {
        if (terrainGenerator == null)
        {
            Debug.LogError("EnemySpawner needs a reference to the TerrainGenerator!");
        }

        // Initialize waves (you can add more waves here)
        waves.Clear();
        waves.Add(new Wave { numEnemies = 5, spawnDelay = 1f });  
        waves.Add(new Wave { numEnemies = 8, spawnDelay = 1.5f });
        waves.Add(new Wave { numEnemies = 12, spawnDelay = 0.8f }); 
    }

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            if (!waitingForWaveEnd)
            {
                SpawnEnemy();
            }
            else
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    waitingForWaveEnd = false;
                    currentWave++;
                    enemiesSpawned = 0;

                    if (currentWave >= waves.Count)
                    {
                        currentWave = 0; // Loop back to the first wave
                    }

                    nextSpawnTime = Time.time + waves[currentWave].spawnDelay;
                }
            }
        }
    }

    void SpawnEnemy()
    {
        List<List<Vector3>> paths = terrainGenerator.paths;

        if (paths.Count == 0)
        {
            Debug.LogError("No paths available for spawning enemies!");
            return;
        }

        int pathIndex = Random.Range(0, paths.Count);
        List<Vector3> path = paths[pathIndex];
        Vector3 spawnPosition = path[0];

        // Determine enemy type based on wave
        GameObject enemyPrefab;
        if (currentWave == 0)
        {
            enemyPrefab = enemyType1;
        }
        else if (currentWave == 1)
        {
            enemyPrefab = enemyType2;
        }
        else if (currentWave == 2)
        {
            enemyPrefab = enemyType3;
        }
        else
        {
            // Randomly select enemy type for waves after the first 3
            int randomEnemyType = Random.Range(0, 3);
            switch (randomEnemyType)
            {
                case 0:
                    enemyPrefab = enemyType1;
                    break;
                case 1:
                    enemyPrefab = enemyType2;
                    break;
                default:
                    enemyPrefab = enemyType3;
                    break;
            }
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.tag = "Enemy";

        EnemyMovement enemyMovement = enemy.AddComponent<EnemyMovement>();
        enemyMovement.path = path;

        enemiesSpawned++;

        if (enemiesSpawned >= waves[currentWave].numEnemies)
        {
            waitingForWaveEnd = true;
        }
        else
        {
            nextSpawnTime = Time.time + waves[currentWave].spawnDelay;
        }
    }
}