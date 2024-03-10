using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileGrid : MonoBehaviour
{
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

        for (int i = 0; i < houseClusterCount; i++) BuildInRandomEmptySpot(housePrefab);

        foreach (Tile tile in tiles) if (!tile.structure && Random.value < grassLandRatio) Build(grasslandPrefab, new List<Tile> { tile });
    }

    private void BuildInRandomEmptySpot(GameObject prefab)
    {
        RandomizeRotation();
        Structure structure = prefab.GetComponent<Structure>();
        Build(prefab, GetCoveredTiles(RandomEmptyTile(structure.coveredTiles), structure.coveredTiles));
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
    }
    public static void ToggleAttributeDisplay(Attribute.AttributeType type)
    {
        isShowingAttributes[(int) type] = !isShowingAttributes[(int) type];
        foreach (Attribute attribute in GetAllAttributes()) attribute.UpdateDisplay();
        foreach (Tile tile in instance.tiles) tile.PositionAttributes();
    }

    public static void ToggleTimerDisplay()
    {
        isShowingTimers = !isShowingTimers;
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
    }

    private static void PlaceAttributes(List<Attribute> attributes, Tile originTile)
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
}
