using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public int seed;
    public Vector3 towerPosition;
    public int numberOfPaths = 3;

    void Start()
    {
        // Generate a random seed if not set
        if (seed == 0)
        {
            seed = Random.Range(0, 100000);
        }

        GenerateTerrain();
        PlaceTower();
        GeneratePaths();
    }

    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, 50, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + seed;
        float yCoord = (float)y / height * scale + seed;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void PlaceTower()
    {
        // Place the tower at the center of the terrain
        towerPosition = new Vector3(width / 2, 0, height / 2);
        // Adjust the y position based on the terrain height
        towerPosition.y = Terrain.activeTerrain.SampleHeight(towerPosition);
        // Instantiate the tower (assuming you have a tower prefab)
        // Instantiate(towerPrefab, towerPosition, Quaternion.identity);
    }

    void GeneratePaths()
    {
        
            for (int i = 0; i < numberOfPaths; i++)
            {
                // Randomly select a side of the terrain
                int side = Random.Range(0, 4);

                Vector3 startPosition;
                switch (side)
                {
                    case 0: // North side
                        startPosition = new Vector3(Random.Range(0, width), 0, height - 1);
                        break;
                    case 1: // East side
                        startPosition = new Vector3(width - 1, 0, Random.Range(0, height));
                        break;
                    case 2: // South side
                        startPosition = new Vector3(Random.Range(0, width), 0, 0);
                        break;
                    default: // West side
                        startPosition = new Vector3(0, 0, Random.Range(0, height));
                        break;
                }

                startPosition.y = Terrain.activeTerrain.SampleHeight(startPosition);
                List<Vector3> path = FindPath(startPosition, towerPosition);

                // Create a new Line Renderer for this path
                GameObject pathRenderer = new GameObject("PathRenderer" + i);
                pathRenderer.transform.parent =
                    transform; // Optional: Parent the line renderer to the terrain generator
                LineRenderer lineRenderer = pathRenderer.AddComponent<LineRenderer>();

                // Set the line renderer properties and draw the path
                DrawPath(path, lineRenderer);
            }
        }

        List<Vector3> FindPath(Vector3 start, Vector3 end)
        {
            // Implement a simple pathfinding algorithm (e.g., A* or Dijkstra)
            // This is a placeholder for the actual pathfinding logic
            List<Vector3> path = new List<Vector3>();
            path.Add(start);
            path.Add(end);
            return path;
        }

        void DrawPath(List<Vector3> path, LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = path.Count;
            lineRenderer.SetPositions(path.ToArray());
        }
    }
