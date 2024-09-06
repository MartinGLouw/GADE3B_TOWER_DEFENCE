using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float spawnInterval = 2f;
    public TerrainGenerator terrainGenerator; // Reference to your TerrainGenerator script

    private float nextSpawnTime = 0f;

    void Start()
    {
        // Make sure you assign the TerrainGenerator reference in the inspector
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
        List<List<Vector3>> paths = terrainGenerator.paths; // Get paths from TerrainGenerator

        if (paths.Count == 0)
        {
            Debug.LogError("No paths available for spawning enemies!");
            return;
        }

        int pathIndex = Random.Range(0, paths.Count);
        List<Vector3> path = paths[pathIndex];

        Vector3 spawnPosition = path[0];
        GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);

        EnemyMovement enemyMovement = enemy.AddComponent<EnemyMovement>();
        enemyMovement.path = path;
    }
}

public class EnemyMovement : MonoBehaviour
{
    public List<Vector3> path;
    public float speed = 2f;
    private int currentWaypoint = 0;

    void Update()
    {
        if (path == null || currentWaypoint >= path.Count)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 target = path[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            currentWaypoint++;
        }
    }
}