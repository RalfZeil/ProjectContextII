using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab, headquartersPrefab;
    [SerializeField] private int gridWidth, gridHeight;

    [HideInInspector] public Tile[,] tiles;
    [HideInInspector] public Tile targetedTile;
    public static bool isShowingAttributes;
    public static TileGrid instance;
    private static int targetRotation = 0;

    private void Start()
    {
        instance = this;
        GenerateStartCity();
    }

    private void GenerateStartCity() //TODO: Actually procedurally generate a city, or have a premade one
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

        Structure structure = Instantiate(headquartersPrefab).GetComponent<Structure>();
        structure.transform.parent = instance.transform;
        structure.transform.position = (new Vector3(0, .3f, 0)) + PositionOfTile(gridWidth / 2, gridHeight / 2);
        tiles[gridWidth / 2, gridHeight / 2].structure = structure;
    }

    private void Update()
    {
        UpdateTarget();

        if (Input.GetKeyDown(KeyCode.R)) targetRotation = (targetRotation + 90) % 360;

        if (Input.GetKeyDown(KeyCode.Tab)) ToggleAttributeDisplay();
    }

    public static void ToggleAttributeDisplay()
    {
        isShowingAttributes = !isShowingAttributes;
        foreach (Attribute attribute in GetAllAttributes()) attribute.UpdateDisplay();
    }

    private void UpdateTarget()
    {
        Tile newTarget = HoveredTile();

        foreach (Tile tile in tiles) tile.Deselect();

        targetedTile = newTarget;
        if (targetedTile)
        {
            foreach(Tile tile in GetTargetedTiles()) if (tile) tile.Select(); 
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

    public static void Build (GameObject structurePrefab)
    {
        foreach (Tile tile in GetTargetedTiles()) tile.Replace();

        Structure structure = Instantiate(structurePrefab).GetComponent<Structure>();
        structure.transform.parent = instance.transform;
        structure.transform.position = (new Vector3(0, .3f, 0)) + instance.targetedTile.transform.position;
        structure.transform.localRotation = Quaternion.Euler(0, targetRotation, 0);
        foreach (Tile tile in GetTargetedTiles()) if (tile != null)
            {
                tile.structure = structure;
                structure.tiles.Add(tile);
            }

        PlaceAttributes(structure.attributes);
    }

    private static void PlaceAttributes(List<Attribute> attributes)
    {
        Tile originTile = instance.targetedTile;
        foreach (Attribute attribute in attributes)
        {
            Vector2Int rotatedCoordinates = RotateCoordinates(attribute.relativeCoordinates);
            int x = originTile.x + rotatedCoordinates.x;
            int y = originTile.y + rotatedCoordinates.y;

            Tile tile = instance.GetTileAt(x, y);
            if (tile) tile.AddAttribute(attribute);
            else attribute.Deactivate();
        }
    }

    public static void Modify(GameObject modificationPrefab)
    {
        if (instance.targetedTile.modification) instance.targetedTile.modification.Remove();

        Modification modification = Instantiate(modificationPrefab).GetComponent<Modification>();
        modification.transform.parent = instance.targetedTile.structure.transform;
        modification.transform.position = (new Vector3(0, .3f, 0)) + instance.targetedTile.transform.position;
        modification.transform.localRotation = Quaternion.Euler(0, targetRotation, 0);

        modification.structure = instance.targetedTile.structure;
        modification.structure.modifications.Add(modification);
        modification.tile = instance.targetedTile;
        instance.targetedTile.modification = modification;

        PlaceAttributes(modification.attributes);

        modification.structure.UpdateAttributeLayering();
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
}
