using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;
    public GameObject enemyType4;
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
                        GenerateNextWave(); 
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
        
        GameObject enemyPrefab;
        if (currentWave < 4) 
        {
            switch (currentWave)
            {
                case 0:
                    enemyPrefab = enemyType4;
                    break;
                case 1:
                    enemyPrefab = enemyType2;
                    break;
                case 2:
                    enemyPrefab = enemyType1;
                    break;
                default:
                    enemyPrefab = enemyType3;
                    break;
            }
        }
        else 
        {
            enemyPrefab = GetRandomEnemyType();
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

    GameObject GetRandomEnemyType()
    {
        int randomEnemyType = Random.Range(0, 3);
        switch (randomEnemyType)
        {
            case 0:
                return enemyType1;
            case 1:
                return enemyType2;
            case 2:
                return enemyType4;
            default:
                return enemyType3;
        }
    }

    void GenerateNextWave()
    {
        int numEnemies = waves[currentWave - 1].numEnemies + Random.Range(2, 5); 
        float spawnDelay = Mathf.Max(0.2f, waves[currentWave - 1].spawnDelay * 0.9f); 

        waves.Add(new Wave { numEnemies = numEnemies, spawnDelay = spawnDelay });
    }
}