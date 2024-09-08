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
    public GameObject RaptorPrefab;
    public GameObject StegoPrefab;
    public GameObject TRexPrefab;
    public Color gridColor = Color.white;
    public Color pathColor = Color.red;
    private HashSet<Vector2Int> validDefenderLocations; // Changed to Vector2Int
    public List<List<Vector3>> paths;
    private int gridSize = 10;
    private float gridSpacing;
    private LineRenderer[,] gridLines;
    public GameObject canvas;
    public GameObject raptorButton;
    public GameObject trexButton;
    public GameObject stegoButton;
    public MeatManager meatManager;
    public IDefender defender;

    void Start()
    {
        canvas.SetActive(false);
        if (seed == 0)
        {
            seed = Random.Range(0, 100000);
        }

        gridSpacing = width / gridSize;
        validDefenderLocations = new HashSet<Vector2Int>();
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
                Vector2Int gridIndex = new Vector2Int(x, z);
                Vector3 location = new Vector3(x * gridSpacing, 0, z * gridSpacing);
                location.y = Terrain.activeTerrain.SampleHeight(location);

                if (!IsOnPath(location))
                {
                    validDefenderLocations.Add(gridIndex);
                    Debug.Log($"Added valid defender location at Grid Index: {gridIndex}");
                }
                else
                {
                    Debug.Log($"Grid Index {gridIndex} is on a path and cannot be used for placing defenders.");
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
                // Only consider horizontal distance to avoid y-axis interference
                float horizontalDistance = Vector2.Distance(new Vector2(location.x, location.z), new Vector2(node.x, node.z));
                if (horizontalDistance < gridSpacing * 0.5f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SpawnRaptor(Vector3 position)
    {
        Vector2Int gridIndex = GetGridIndex(position);
        if (validDefenderLocations.Contains(gridIndex))
        {
            Vector3 gridPosition = GetGridPosition(gridIndex);
            Instantiate(RaptorPrefab, gridPosition, Quaternion.identity);
            validDefenderLocations.Remove(gridIndex);
            Debug.Log($"Defender placed at Grid Index: {gridIndex} -> Position: {gridPosition}");
        }
        else
        {
            Debug.Log($"Failed to place defender at Grid Index: {gridIndex}. The location is either on a path or already occupied.");
        }
    }
    public void SpawnTrex(Vector3 position)
    {
        Vector2Int gridIndex = GetGridIndex(position);
        if (validDefenderLocations.Contains(gridIndex))
        {
            Vector3 gridPosition = GetGridPosition(gridIndex);
            Instantiate(TRexPrefab, gridPosition, Quaternion.identity);
            validDefenderLocations.Remove(gridIndex);
            Debug.Log($"Defender placed at Grid Index: {gridIndex} -> Position: {gridPosition}");
        }
        else
        {
            Debug.Log($"Failed to place defender at Grid Index: {gridIndex}. The location is either on a path or already occupied.");
        }
    }
    public void SpawnStego(Vector3 position)
    {
        Vector2Int gridIndex = GetGridIndex(position);
        if (validDefenderLocations.Contains(gridIndex))
        {
            Vector3 gridPosition = GetGridPosition(gridIndex);
            Instantiate(StegoPrefab, gridPosition, Quaternion.identity);
            validDefenderLocations.Remove(gridIndex);
            Debug.Log($"Defender placed at Grid Index: {gridIndex} -> Position: {gridPosition}");
        }
        else
        {
            Debug.Log($"Failed to place defender at Grid Index: {gridIndex}. The location is either on a path or already occupied.");
        }
    }

    // void PlaceDefender(Vector3 position)
    // {
    //     
    //     Vector2Int gridIndex = GetGridIndex(position);
    //     if (validDefenderLocations.Contains(gridIndex))
    //     {
    //         Vector3 gridPosition = GetGridPosition(gridIndex);
    //         Instantiate(DefenderPrefab, gridPosition, Quaternion.identity);
    //         validDefenderLocations.Remove(gridIndex);
    //         Debug.Log($"Defender placed at Grid Index: {gridIndex} -> Position: {gridPosition}");
    //     }
    //     else
    //     {
    //         Debug.Log($"Failed to place defender at Grid Index: {gridIndex}. The location is either on a path or already occupied.");
    //     }
    // }
    void PlaceDefender(Vector3 position, IDefender defender)
    {
        Vector2Int gridIndex = GetGridIndex(position);
        if (validDefenderLocations.Contains(gridIndex))
        {
            if (meatManager.meat >= defender.MeatCost)
            {
                Vector3 gridPosition = GetGridPosition(gridIndex);
                Instantiate(DefenderPrefab, gridPosition, Quaternion.identity);
                validDefenderLocations.Remove(gridIndex);
                meatManager.AddMeat(-defender.MeatCost); // Deduct the meat cost
                Debug.Log($"Defender placed at Grid Index: {gridIndex} -> Position: {gridPosition}. Meat remaining: {meatManager.meat}");
            }
            else
            {
                Debug.Log("Not enough meat to place defender.");
            }
        }
        else
        {
            Debug.Log($"Failed to place defender at Grid Index: {gridIndex}. The location is either on a path or already occupied.");
        }
    }

    Vector2Int GetGridIndex(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / gridSpacing);
        int z = Mathf.RoundToInt(position.z / gridSpacing);
        Vector2Int gridIndex = new Vector2Int(x, z);
        Debug.Log($"Converted Position {position} to Grid Index {gridIndex}");
        return gridIndex;
    }

    Vector3 GetGridPosition(Vector2Int gridIndex)
    {
        Vector3 gridPosition = new Vector3(gridIndex.x * gridSpacing, Terrain.activeTerrain.SampleHeight(new Vector3(gridIndex.x * gridSpacing, 0, gridIndex.y * gridSpacing)), gridIndex.y * gridSpacing);
        Debug.Log($"Converted Grid Index {gridIndex} to Position {gridPosition}");
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
                //Call UI
                PlaceDefender(hitPoint, new());
               
            }
        }
    }

    public void ChosenButton()
    {
        
    }

    void PlaceTower()
    {
        Vector2Int towerGridIndex = new Vector2Int(gridSize / 2, gridSize / 2);
        towerPosition = GetGridPosition(towerGridIndex);
        Instantiate(Tower, towerPosition, Quaternion.identity);
        Debug.Log($"Tower placed at {towerPosition}");
    }

    void GeneratePaths()
    {
        float pathHeightOffset = 1.5f; // Adjust this value as necessary

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

            startPosition.y = Terrain.activeTerrain.SampleHeight(startPosition) + pathHeightOffset; // Add offset
            List<Vector3> path = FindPath(startPosition, towerPosition);
            paths.Add(path);

            // Create a new LineRenderer for the path
            GameObject pathLineObj = new GameObject($"Path_{i}");
            LineRenderer pathLine = pathLineObj.AddComponent<LineRenderer>();

            pathLine.startWidth = 0.5f;
            pathLine.endWidth = 0.5f;
            pathLine.positionCount = path.Count;
            pathLine.material = new Material(Shader.Find("Sprites/Default"));
            pathLine.startColor = pathColor;
            pathLine.endColor = pathColor;

            // Set positions for the path
            for (int j = 0; j < path.Count; j++)
            {
                Vector3 nodePosition = path[j];
                nodePosition.y = Terrain.activeTerrain.SampleHeight(nodePosition) + pathHeightOffset; // Add offset
                pathLine.SetPosition(j, nodePosition);
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

        // Simple pathfinding: move horizontally first, then vertically
        Vector3 current = GetGridPosition(new Vector2Int(startX, startZ));
        path.Add(current);

        while (startX != endX || startZ != endZ)
        {
            if (startX < endX) startX++;
            else if (startX > endX) startX--;

            if (startZ < endZ) startZ++;
            else if (startZ > endZ) startZ--;

            current = GetGridPosition(new Vector2Int(startX, startZ));
            path.Add(current);
        }

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
        Vector2Int gridIndex = GetGridIndex(position);
        if (gridLines[gridIndex.x, gridIndex.y] != null)
        {
            gridLines[gridIndex.x, gridIndex.y].startColor = color;
            gridLines[gridIndex.x, gridIndex.y].endColor = color;
        }
    }
}
