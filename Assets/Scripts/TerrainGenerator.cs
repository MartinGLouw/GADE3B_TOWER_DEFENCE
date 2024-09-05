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
    public Color hoverColor = Color.green; // Color to highlight the hovered cell
    private HashSet<Vector2Int> validDefenderLocations;
    private List<List<Vector3>> paths;
    private int gridSize = 10;
    private float gridSpacing;
    private LineRenderer[,] gridLines;
    private Vector2Int? lastHoveredGridIndex = null; // Track the last hovered grid index

    void Start()
    {
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

    void Update()
    {
        HandleMouseHover(); // Call hover handling every frame

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

    void HandleMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = Terrain.activeTerrain.SampleHeight(hitPoint);

            Vector2Int gridIndex = GetGridIndex(hitPoint);

            // Reset the color of the previously hovered cell
            if (lastHoveredGridIndex.HasValue && lastHoveredGridIndex.Value != gridIndex)
            {
                ColorGrid(GetGridPosition(lastHoveredGridIndex.Value), gridColor);
            }

            // Highlight the current hovered grid cell
            if (gridIndex.x >= 0 && gridIndex.x < gridSize && gridIndex.y >= 0 && gridIndex.y < gridSize)
            {
                ColorGrid(GetGridPosition(gridIndex), hoverColor);
                lastHoveredGridIndex = gridIndex;
            }
        }
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
                float horizontalDistance = Vector2.Distance(new Vector2(location.x, location.z), new Vector2(node.x, node.z));
                if (horizontalDistance < gridSpacing * 0.5f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void PlaceDefender(Vector3 position)
    {
        Vector2Int gridIndex = GetGridIndex(position);
        if (validDefenderLocations.Contains(gridIndex))
        {
            Vector3 gridPosition = GetGridPosition(gridIndex);
            Instantiate(DefenderPrefab, gridPosition, Quaternion.identity);
            validDefenderLocations.Remove(gridIndex);
            Debug.Log($"Defender placed at Grid Index: {gridIndex} -> Position: {gridPosition}");
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

    void PlaceTower()
    {
        Vector2Int towerGridIndex = new Vector2Int(gridSize / 2, gridSize / 2);
        towerPosition = GetGridPosition(towerGridIndex);
        Instantiate(Tower, towerPosition, Quaternion.identity);
        Debug.Log($"Tower placed at {towerPosition}");
    }

    void GeneratePaths()
    {
        float pathHeightOffset = 1.5f;

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

            startPosition.y = Terrain.activeTerrain.SampleHeight(startPosition) + pathHeightOffset;
            List<Vector3> path = FindPath(startPosition, towerPosition);
            paths.Add(path);

            GameObject pathLineObj = new GameObject($"Path_{i}");
            LineRenderer pathLine = pathLineObj.AddComponent<LineRenderer>();
            pathLine.startWidth = 0.5f;
            pathLine.endWidth = 0.5f;
            pathLine.positionCount = path.Count;
            pathLine.material = new Material(Shader.Find("Sprites/Default"));
            pathLine.startColor = pathColor;
            pathLine.endColor = pathColor;

            for (int j = 0; j < path.Count; j++)
            {
                Vector3 nodePosition = path[j];
                nodePosition.y = Terrain.activeTerrain.SampleHeight(nodePosition) + pathHeightOffset;
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

        while (startX != endX || startZ != endZ)
        {
            if (startX < endX) startX++;
            else if (startX > endX) startX--;

            if (startZ < endZ) startZ++;
            else if (startZ > endZ) startZ--;

            Vector3 currentPosition = new Vector3(startX * gridSpacing, Terrain.activeTerrain.SampleHeight(new Vector3(startX * gridSpacing, 0, startZ * gridSpacing)), startZ * gridSpacing);
            path.Add(currentPosition);
        }

        return path;
    }

    void DrawGrid()
    {
        GameObject gridParent = new GameObject("Grid");

        for (int i = 0; i <= gridSize; i++)
        {
            for (int j = 0; j <= gridSize; j++)
            {
                Vector3 start = new Vector3(i * gridSpacing, Terrain.activeTerrain.SampleHeight(new Vector3(i * gridSpacing, 0, 0)), 0);
                Vector3 end = new Vector3(i * gridSpacing, Terrain.activeTerrain.SampleHeight(new Vector3(i * gridSpacing, 0, gridSize * gridSpacing)), gridSize * gridSpacing);

                GameObject gridLineObj = new GameObject($"GridLine_{i}_{j}");
                LineRenderer line = gridLineObj.AddComponent<LineRenderer>();
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                line.positionCount = 2;
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.startColor = gridColor;
                line.endColor = gridColor;
                line.SetPosition(0, start);
                line.SetPosition(1, end);

                gridLines[i, j] = line;
            }
        }

        Debug.Log("Grid drawn.");
    }

    void ColorGrid(Vector3 position, Color color)
    {
        LineRenderer line = gridLines[Mathf.RoundToInt(position.x / gridSpacing), Mathf.RoundToInt(position.z / gridSpacing)];
        if (line != null)
        {
            line.startColor = color;
            line.endColor = color;
        }
    }
}
