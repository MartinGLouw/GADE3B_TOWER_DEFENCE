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
    public GameObject Tower;
    public GameObject DefenderPrefab;
    public Color gridColor = Color.white;
    public Color pathColor = Color.red;
    private HashSet<Vector3> validDefenderLocations;
    private List<List<Vector3>> paths;
    private int gridSize = 10;
    private float gridSpacing;
    private LineRenderer[,] gridLines;

    void Start()
    {
        if (seed == 0)
        {
            seed = Random.Range(0, 100000);
        }

        gridSpacing = width / gridSize;
        validDefenderLocations = new HashSet<Vector3>();
        paths = new List<List<Vector3>>();
        gridLines = new LineRenderer[gridSize + 1, gridSize + 1];

        GenerateTerrain();
        PlaceTower();
        GeneratePaths();
        PopulateValidDefenderLocations();
        DrawGrid();

        Debug.Log("Terrain generation complete. Paths generated and valid defender locations set.");
    }

    void PopulateValidDefenderLocations()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 location = new Vector3(x * gridSpacing, 0, z * gridSpacing);
                location.y = Terrain.activeTerrain.SampleHeight(location);

                if (!IsOnPath(location))
                {
                    validDefenderLocations.Add(location);
                    Debug.Log($"Added valid defender location at {location}");
                }
                else
                {
                    Debug.Log($"Location {location} is on a path and cannot be used for placing defenders.");
                }
            }
        }
    }

    bool IsOnPath(Vector3 location)
    {
        foreach (List<Vector3> path in paths)
        {
            foreach (Vector3 node in path)
            {
                if (Vector3.Distance(location, node) < gridSpacing * 0.5f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void PlaceDefender(Vector3 position)
    {
        Vector3 gridPosition = GetNearestGridPosition(position);
        if (validDefenderLocations.Contains(gridPosition))
        {
            Instantiate(DefenderPrefab, gridPosition, Quaternion.identity);
            validDefenderLocations.Remove(gridPosition);
            Debug.Log($"Defender placed at {gridPosition}");
        }
        else
        {
            Debug.Log($"Failed to place defender at {gridPosition}. The location is either on a path or already occupied.");
        }
    }

    Vector3 GetNearestGridPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / gridSpacing);
        int z = Mathf.RoundToInt(position.z / gridSpacing);
        Vector3 gridPosition = new Vector3(x * gridSpacing, position.y, z * gridSpacing);
        Debug.Log($"Nearest grid position for {position} is {gridPosition}");
        return gridPosition;
    }

    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
        Debug.Log("Terrain data generated.");
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
        Debug.Log("Heights generated for terrain.");
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + seed;
        float yCoord = (float)y / height * scale + seed;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.y = Terrain.activeTerrain.SampleHeight(hitPoint);

                Debug.Log($"Mouse clicked at {hitPoint}");
                PlaceDefender(hitPoint);
            }
        }
    }

    void PlaceTower()
    {
        towerPosition = new Vector3(width / 2, 0, height / 2);
        towerPosition.y = Terrain.activeTerrain.SampleHeight(towerPosition);
        Instantiate(Tower, towerPosition, Quaternion.identity);
        Debug.Log($"Tower placed at {towerPosition}");
    }

    void GeneratePaths()
    {
        for (int i = 0; i < numberOfPaths; i++)
        {
            int side = Random.Range(0, 4);

            Vector3 startPosition;
            switch (side)
            {
                case 0:
                    startPosition = new Vector3(Random.Range(0, gridSize) * gridSpacing, 0, height - 1);
                    break;
                case 1:
                    startPosition = new Vector3(width - 1, 0, Random.Range(0, gridSize) * gridSpacing);
                    break;
                case 2:
                    startPosition = new Vector3(Random.Range(0, gridSize) * gridSpacing, 0, 0);
                    break;
                default:
                    startPosition = new Vector3(0, 0, Random.Range(0, gridSize) * gridSpacing);
                    break;
            }

            startPosition.y = Terrain.activeTerrain.SampleHeight(startPosition);
            List<Vector3> path = FindPath(startPosition, towerPosition);
            paths.Add(path);

            foreach (Vector3 node in path)
            {
                ColorGrid(node, pathColor);
            }

            Debug.Log($"Path {i} generated from {startPosition} to {towerPosition}");
        }
    }

    List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        List<Vector3> path = new List<Vector3>();

        int startX = Mathf.RoundToInt(start.x / gridSpacing);
        int startZ = Mathf.RoundToInt(start.z / gridSpacing);
        int endX = Mathf.RoundToInt(end.x / gridSpacing);
        int endZ = Mathf.RoundToInt(end.z / gridSpacing);

        // Simple A* algorithm for pathfinding on the grid
        Vector3 current = start;
        while (startX != endX || startZ != endZ)
        {
            path.Add(current);

            if (startX < endX) startX++;
            else if (startX > endX) startX--;

            if (startZ < endZ) startZ++;
            else if (startZ > endZ) startZ--;

            current = new Vector3(startX * gridSpacing, Terrain.activeTerrain.SampleHeight(current), startZ * gridSpacing);
        }
        path.Add(end);

        return path;
    }

    void DrawGrid()
    {
        for (int x = 0; x <= gridSize; x++)
        {
            for (int z = 0; z <= gridSize; z++)
            {
                Vector3 start = new Vector3(x * gridSpacing, 0, z * gridSpacing);
                start.y = Terrain.activeTerrain.SampleHeight(start);
                DrawLine(start, gridColor, x, z);
            }
        }
    }

    void DrawLine(Vector3 position, Color color, int x, int z)
    {
        GameObject lineObj = new GameObject($"GridLine_{x}_{z}");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 4;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.SetPosition(0, position);
        lineRenderer.SetPosition(1, new Vector3(position.x + gridSpacing, position.y, position.z));
        lineRenderer.SetPosition(2, new Vector3(position.x + gridSpacing, position.y, position.z + gridSpacing));
        lineRenderer.SetPosition(3, new Vector3(position.x, position.y, position.z + gridSpacing));

        gridLines[x, z] = lineRenderer;
    }

    void ColorGrid(Vector3 position, Color color)
    {
        int x = Mathf.RoundToInt(position.x / gridSpacing);
        int z = Mathf.RoundToInt(position.z / gridSpacing);
        if (gridLines[x, z] != null)
        {
            gridLines[x, z].startColor = color;
            gridLines[x, z].endColor = color;
        }
    }
}
