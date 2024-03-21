using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private CardSettings cardSettings;
    [SerializeField] private GameObject tilePrefab, factoryPrefab, housePrefab, grasslandPrefab;
    [SerializeField] private List<GameObject> startBuildingPrefabs;
    [SerializeField] private int gridWidth, gridHeight, maxUpdateCycles, factoryCount, houseClusterCount, houseClusterSize, houseClusterRandomness;
    [SerializeField] private float grassLandRatio;

    [HideInInspector] public Tile[,] tiles;
    [HideInInspector] public Tile targetedTile;
    public static bool attributeHasChangedSinceUpdate = false, isShowingTimers = false;
    public static List<bool> isShowingAttributes = new(){false, false, false};
    public static TileGrid instance;
    public static int targetRotation = 0;

    [HideInInspector] public delegate void BuildEvent(Structure structure);
    public static event BuildEvent OnBuild;
    [HideInInspector] public delegate void ModifyEvent(Modification modification);
    public static event ModifyEvent OnModify;
    [HideInInspector] public delegate void AttributeToggleEvent();
    public static event AttributeToggleEvent OnAttributeToggle;

    private void Awake()
    {
        Card.settings = cardSettings;
    }

    private void Start()
    {
        instance = this;
        GenerateTileGrid();
        GenerateStartCity();
    }

    private void GenerateTileGrid()
    {
        tiles = new Tile[gridWidth, gridHeight];

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Transform newTile = Instantiate(tilePrefab).transform;
                newTile.parent = transform;
                newTile.localPosition = PositionOfTile(i, j);
                tiles[i, j] = newTile.GetComponent<Tile>();
                tiles[i, j].x = i;
                tiles[i, j].y = j;
            }
        }
    }
    private void GenerateStartCity()
    {
        foreach (GameObject prefab in instance.startBuildingPrefabs) BuildInRandomEmptySpot(prefab);

        for (int i = 0; i < factoryCount; i++) BuildInRandomEmptySpot(factoryPrefab);

        for (int i = 0; i < houseClusterCount; i++)
        {
            Tile origin = BuildInRandomEmptySpot(housePrefab);
            List<Tile> candidateTiles = new();
            foreach (Tile tile in SurroundingTiles(origin)) if (!tile.structure) candidateTiles.Add(tile);
            int houseCount = houseClusterSize + Random.Range(0, houseClusterRandomness + 1);
            for (int j = 0; j < houseCount; j++)
            {
                if (candidateTiles.Count == 0) break;
                Tile newBuildTile = candidateTiles[Random.Range(0, candidateTiles.Count)];
                candidateTiles.Remove(newBuildTile);
                Build(housePrefab, new List<Tile> { newBuildTile });
                foreach (Tile tile in SurroundingTiles(newBuildTile)) if (!tile.structure && !candidateTiles.Contains(tile)) candidateTiles.Add(tile);
            }
        }

        foreach (Tile tile in tiles) if (!tile.structure && Random.value < grassLandRatio) Build(grasslandPrefab, new List<Tile> { tile });
    }

    private Tile BuildInRandomEmptySpot(GameObject prefab)
    {
        RandomizeRotation();
        Structure structure = prefab.GetComponent<Structure>();
        Tile randomTile = RandomEmptyTile(structure.coveredTiles);
        Build(prefab, GetCoveredTiles(randomTile, structure.coveredTiles));
        return randomTile;
    }

    private static Tile RandomTile()
    {
        return instance.tiles[Random.Range(0, instance.gridWidth), Random.Range(0, instance.gridHeight)];
    }

    private static Tile RandomEmptyTile(List<Vector2Int> coveredCoordinates)
    {
        List<Tile> candidateTiles = new();

        foreach(Tile tile in instance.tiles)
        {
            if (IsEmptySpace(GetCoveredTiles(tile, coveredCoordinates))) candidateTiles.Add(tile);
        }

        return candidateTiles[Random.Range(0, candidateTiles.Count)];
    }

    private static void RandomizeRotation()
    {
        targetRotation = Random.Range(0, 4) * 90;
    }

    private static List<Tile> SurroundingTiles(Tile tile)
    {
        List<Tile> surroundingTiles = new();
        if (tile.x > 0) surroundingTiles.Add(instance.GetTileAt(tile.x - 1, tile.y));
        if (tile.x < instance.gridWidth - 1) surroundingTiles.Add(instance.GetTileAt(tile.x + 1, tile.y));
        if (tile.y > 0) surroundingTiles.Add(instance.GetTileAt(tile.x, tile.y - 1));
        if (tile.y > instance.gridHeight - 1) surroundingTiles.Add(instance.GetTileAt(tile.x, tile.y + 1));

        return surroundingTiles;
    }

    private void Update()
    {
        UpdateTarget();

        if (Input.GetKeyDown(KeyCode.R)) targetRotation = (targetRotation + 90) % 360;

        if (Input.GetKeyDown(KeyCode.Tab)) ToggleAttributeDisplay();
        if (Input.GetKeyDown(KeyCode.Alpha1)) ToggleAttributeDisplay(Attribute.AttributeType.Negative);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ToggleAttributeDisplay(Attribute.AttributeType.PositiveNature);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ToggleAttributeDisplay(Attribute.AttributeType.PositiveCivilisation);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ToggleTimerDisplay();
    }

    public static void ToggleAttributeDisplay()
    {
        bool toggle = true;
        foreach (bool b in isShowingAttributes) if (b) toggle = false;
        for (int i = 0; i < isShowingAttributes.Count; i++) isShowingAttributes[i] = toggle;

        foreach (Attribute attribute in GetAllAttributes()) attribute.UpdateDisplay();
        foreach (Tile tile in instance.tiles) tile.PositionAttributes();

        OnAttributeToggle?.Invoke();
    }
    public static void ToggleAttributeDisplay(Attribute.AttributeType type)
    {
        isShowingAttributes[(int) type] = !isShowingAttributes[(int) type];
        foreach (Attribute attribute in GetAllAttributes()) attribute.UpdateDisplay();
        foreach (Tile tile in instance.tiles) tile.PositionAttributes();

        OnAttributeToggle?.Invoke();
    }

    public static void ToggleTimerDisplay()
    {
        isShowingTimers = !isShowingTimers;

        OnAttributeToggle?.Invoke();
    }

    private void UpdateTarget()
    {
        Tile newTarget = HoveredTile();

        foreach (Tile tile in tiles) tile.Deselect();

        targetedTile = newTarget;
        if (targetedTile)
        {
            if (!Card.heldCard) foreach(Tile tile in GetTargetedTiles()) if (tile) tile.Select(); 
        }

    }

    private Tile HoveredTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tiles"))) return hit.transform.GetComponentInParent<Tile>();
        return null;
    }

    public static bool IsTileTargeted()
    {
        return instance.targetedTile != null;
    }

    public static List<Tile> GetTargetedTiles()
    {
        List<Tile> targetTiles = new();
        if (!IsTileTargeted()) return targetTiles;

        Tile originTile = instance.targetedTile;
        targetTiles.Add(originTile);

        if(Card.heldCard) foreach(Vector2Int coordinates in Card.heldCard.TargetedTiles())
        {
            Vector2Int rotatedCoordinates = RotateCoordinates(coordinates);
            int i = originTile.x + rotatedCoordinates.x;
            int j = originTile.y + rotatedCoordinates.y;
            targetTiles.Add(instance.GetTileAt(i, j));
        }

        return targetTiles;
    }

    private static List<Tile> GetCoveredTiles(Tile originTile, List<Vector2Int> coveredCoordinates)
    {
        List<Tile> coveredTiles = new();
        coveredTiles.Add(originTile);

        foreach (Vector2Int coordinates in coveredCoordinates)
            {
                Vector2Int rotatedCoordinates = RotateCoordinates(coordinates);
                int i = originTile.x + rotatedCoordinates.x;
                int j = originTile.y + rotatedCoordinates.y;
                coveredTiles.Add(instance.GetTileAt(i, j));
            }

        return coveredTiles;
    }

    private static Vector2Int RotateCoordinates(Vector2Int coordinates)
    {
        if (targetRotation == 0) return coordinates;
        if (targetRotation == 90) return new Vector2Int(coordinates.y, -1 * coordinates.x);
        if (targetRotation == 180) return -1 * coordinates;
        return new Vector2Int(-1 * coordinates.y, coordinates.x);
    }

    private Tile GetTileAt(int x, int y)
    {
        if (x >= 0 && x < instance.gridWidth && y >= 0 && y < instance.gridHeight) return instance.tiles[x, y];
        return null;
    }

    public static void Build (GameObject structurePrefab, List<Tile> tiles)
    {
        foreach (Tile tile in tiles) tile.Replace();

        Structure structure = Instantiate(structurePrefab).GetComponent<Structure>();
        structure.transform.parent = instance.transform;
        structure.transform.position = (new Vector3(0, .3f, 0)) + tiles[0].transform.position;
        structure.transform.localRotation = Quaternion.Euler(0, targetRotation, 0);
        foreach (Tile tile in tiles) if (tile != null)
            {
                tile.structure = structure;
                structure.tiles.Add(tile);
            }

        PlaceAttributes(structure.attributes, tiles[0]);

        UpdateAttributeEffects();

        OnBuild?.Invoke(structure);
    }

    public static void PlaceAttributes(List<Attribute> attributes, Tile originTile)
    {
        foreach (Attribute attribute in attributes)
        {
            Vector2Int rotatedCoordinates = RotateCoordinates(attribute.relativeCoordinates);
            int x = originTile.x + rotatedCoordinates.x;
            int y = originTile.y + rotatedCoordinates.y;

            Tile tile = instance.GetTileAt(x, y);
            if (tile) tile.AddAttribute(attribute);
            else attribute.UpdateDisplay();
        }
    }

    public static void Modify(GameObject modificationPrefab)
    {
        if (instance.targetedTile.modification) instance.targetedTile.modification.Remove();

        Modification modification = Instantiate(modificationPrefab).GetComponent<Modification>();
        modification.transform.parent = instance.targetedTile.structure.transform;
        modification.transform.position = (new Vector3(0, .3f, 0)) + instance.targetedTile.transform.position;
        modification.transform.localRotation = Quaternion.Euler(0, targetRotation, 0);

        modification.structure.modifications.Add(modification);
        modification.tile = instance.targetedTile;
        instance.targetedTile.modification = modification;

        PlaceAttributes(modification.attributes, instance.targetedTile);

        modification.structure.UpdateAttributeLayering();

        UpdateAttributeEffects();

        OnModify?.Invoke(modification);
    }

    public static void UpdateAttributeEffects()
    {
        int cycleCount = 0;

        do
        {
            attributeHasChangedSinceUpdate = false;
            cycleCount++;
            foreach (Structure structure in GetStructures()) structure.UpdateAttributeBonus();
        }
        while (attributeHasChangedSinceUpdate && cycleCount < instance.maxUpdateCycles);

        foreach (Tile tile in instance.tiles)
        {
            tile.SortAttributes();
            tile.PositionAttributes();
        }

        if (cycleCount == instance.maxUpdateCycles) Debug.Log("Warning: Infinite attribute effect loop");
    }

    private Vector3 PositionOfTile(int x, int y)
    {
        return new Vector3(x, 0, y);
    }

    public static bool IsValidPlacement()
    {
        foreach(Tile tile in GetTargetedTiles())
        {
            if (tile == null) return false;
            if (tile.structure && tile.structure.isPermanent) return false;
        }

        return true;
    }

    private static bool IsEmptySpace(List<Tile> tiles)
    {
        foreach (Tile tile in tiles) if (!tile || tile.structure) return false;
        return true;
    }

    public static List<Structure> GetStructures()
    {
        HashSet<Structure> structures = new();
        foreach (Tile tile in instance.tiles) if(tile.structure) structures.Add(tile.structure);
        return structures.ToList<Structure>();
    }

    public static List<StructureFunction> GetStructureFunctions()
    {
        HashSet<StructureFunction> functions = new();
        foreach (Tile tile in instance.tiles) if (tile.structure)
            {
                StructureFunction function = tile.structure.GetComponent<StructureFunction>();
                if (function) functions.Add(function);
            }
        return functions.ToList<StructureFunction>();
    }

    public static List<Attribute> GetAllAttributes()
    {
        List<Attribute> attributes = new();
        foreach (Structure structure in GetStructures()) attributes.AddRange(structure.GetAllAttributes());
        return attributes;
    }

    public static int[,] GetNatureGrid()
    {
        int[,] natureGrid = new int[instance.gridWidth, instance.gridHeight];

        for(int x = 0; x < instance.gridWidth; x++)
        {
            for (int y = 0; y < instance.gridHeight; y++)
            {
                foreach (Attribute attribute in instance.tiles[x, y].attributes)
                {
                    if (attribute.type == Attribute.AttributeType.PositiveNature) natureGrid[x, y]++;
                }
            }
        }

        return natureGrid;
    }

    public static bool[,] GetBoolNatureGrid()
    {
        bool[,] natureGrid = new bool[instance.gridWidth, instance.gridHeight];

        for (int x = 0; x < instance.gridWidth; x++)
        {
            for (int y = 0; y < instance.gridHeight; y++)
            {
                foreach (Attribute attribute in instance.tiles[x, y].attributes)
                {
                    if (attribute.type == Attribute.AttributeType.PositiveNature) natureGrid[x, y] = true;
                }
            }
        }

        return natureGrid;
    }

    public static int countArea(int x, int y, int[,] grid, bool[,] visited)
    {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1) || visited[x, y]) return 0;
        visited[x, y] = true;

        if (grid[x, y] == 0) return 0;

        int area = grid[x, y];
        area += countArea(x + 1, y, grid, visited);
        area += countArea(x, y + 1, grid, visited);
        area += countArea(x - 1, y, grid, visited);
        area += countArea(x, y - 1, grid, visited);
        return area;
    }

    public static List<Tile> GetTilesWithDistanceFromHabitat(int habitatX, int habitatY, int distance)
    {
        int[,] distanceGrid = GetDistanceGrid(habitatX, habitatY, GetBoolNatureGrid());
        List<Tile> tiles = new();
        for (int x = 0; x < distanceGrid.GetLength(0); x++)
        {
            for (int y = 0; y < distanceGrid.GetLength(1); y++)
            {
                if (distanceGrid[x, y] == distance) tiles.Add(instance.GetTileAt(x, y));
            }
        }
        return tiles;
    }

    public static int[,] GetDistanceGrid(int originX, int originY, bool[,] baseGrid)
    {
        int[,] distanceGrid = new int[baseGrid.GetLength(0), baseGrid.GetLength(1)];
        for (int x = 0; x < distanceGrid.GetLength(0); x++) for (int y = 0; y < distanceGrid.GetLength(1); y++) distanceGrid[x, y] = int.MaxValue;
        distanceGrid[originX, originY] = 0;

        int distance = 0;
        bool isDone = false;
        while(!isDone)
        {
            for (int x = 0; x < distanceGrid.GetLength(0); x++)
            {
                for (int y = 0; y < distanceGrid.GetLength(1); y++) if (distanceGrid[x, y] == distance)
                    {
                        DistanceGridStep(x, y, distanceGrid, baseGrid);
                    }
            }

            distance++;
            isDone = true;
            foreach (int d in distanceGrid) if (d == int.MaxValue) isDone = false;
        }

        return distanceGrid;
    }

    private static void DistanceGridStep(int x, int y, int[,] distanceGrid, bool[,] baseGrid)
    {
        (int, int)[] nextSteps = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
        foreach ((int, int) step in nextSteps)
        {
            if (IsOnGrid(step.Item1, step.Item2, baseGrid) && distanceGrid[step.Item1, step.Item2] == int.MaxValue)
            {
                if (baseGrid[step.Item1, step.Item2])
                {
                    distanceGrid[step.Item1, step.Item2] = distanceGrid[x, y];
                    DistanceGridStep(step.Item1, step.Item2, distanceGrid, baseGrid);
                } else
                {
                    distanceGrid[step.Item1, step.Item2] = distanceGrid[x, y] + 1;
                }
            }
        }
    }

    private static void RecursiveDistanceGrid(int x, int y, int[,] distanceGrid, bool[,] baseGrid, int currentDistance)
    {
        distanceGrid[x, y] = currentDistance;

        (int, int)[] nextSteps = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
        foreach ((int, int) step in nextSteps) if (IsOnGrid(step.Item1, step.Item2, baseGrid))
            {
                int stepDistance = currentDistance + (baseGrid[step.Item1, step.Item2] ? 0 : 1);
                if (stepDistance < distanceGrid[step.Item1, step.Item2]) RecursiveDistanceGrid(step.Item1, step.Item2, distanceGrid, baseGrid, stepDistance);
            }
    }

    private static bool IsOnGrid(int x, int y, bool[,] baseGrid)
    {
        return (x >= 0 && y >= 0 && x < baseGrid.GetLength(0) && y < baseGrid.GetLength(1));
    }

    private static bool[,] ExpandedGridByOne(bool [,] grid)
    {
        bool[,] newGrid = grid.Clone() as bool[,];

        for (int x = 0; x < newGrid.GetLength(0); x++)
        {
            for (int y = 0; y < newGrid.GetLength(1); y++) if (newGrid[x, y])
            {
                if (x > 0)                          newGrid[x - 1, y] = true;
                if (x < newGrid.GetLength(0) - 1)   newGrid[x + 1, y] = true;
                if (y > 0)                          newGrid[x, y - 1] = true;
                if (y < newGrid.GetLength(1) - 1)   newGrid[x, y + 1] = true;
            }
        }

        return newGrid;
    }

    public static List<Structure> GetHabitats() //PLACEHOLDER IMPLEMENTATION
    {
        List<Structure> habitats = new();
        foreach (Structure structure in GetStructures())
            if (structure.color == CardSettings.CardColor.Nature && structure.GetComponent<MissionStructure>()) habitats.Add(structure);
        return habitats;
    }
}
