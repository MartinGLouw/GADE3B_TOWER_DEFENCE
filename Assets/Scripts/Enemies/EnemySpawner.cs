using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float spawnInterval = 10f;
    public TerrainGenerator terrainGenerator; 

    private float nextSpawnTime = 0f;
    

    void Start()
    {
        
        if (terrainGenerator == null)
        {
            Debug.LogError("EnemySpawner needs a reference to the TerrainGenerator!");
        }
    }

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        int RandomRange = Random.Range(0, 3);
        List<List<Vector3>> paths = terrainGenerator.paths; //Get paths from TerrainGenerator

        if (paths.Count == 0)
        {
            Debug.LogError("No paths available for spawning enemies!");
            return;
        }

        if (RandomRange == 1)
        {
            
        }
        else if (RandomRange == 2)
        {
            
        }
        else if (RandomRange == 3)
        {
            
        }
        
        
        int pathIndex = Random.Range(0, paths.Count);
        List<Vector3> path = paths[pathIndex];

        Vector3 spawnPosition = path[0];
        GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);

        EnemyMovement enemyMovement = enemy.AddComponent<EnemyMovement>();
        enemyMovement.path = path;
    }
}

