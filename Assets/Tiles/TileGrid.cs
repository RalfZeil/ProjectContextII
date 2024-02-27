using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab, headquartersPrefab;
    [SerializeField] private int gridWidth, gridHeight;

    private Tile[,] tiles;
    public Transform targetedTile = null;
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
    }

    private void UpdateTarget()
    {
        Transform newTarget = HoveredTile();

        foreach (Tile tile in tiles) tile.Deselect();

        targetedTile = newTarget;
        if (targetedTile)
        {
            foreach(Tile tile in GetTargetedTiles()) if (tile) tile.Select(); 
        }

    }

    private Transform HoveredTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tiles"))) return hit.transform;
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

        Tile originTile = instance.targetedTile.GetComponent<Tile>();
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
        structure.transform.position = (new Vector3(0, .3f, 0)) + instance.targetedTile.position;
        structure.transform.localRotation *= Quaternion.Euler(0, targetRotation, 0);
        foreach (Tile tile in GetTargetedTiles()) if (tile != null) tile.structure = structure;

        instance.PlaceAttributes(structure);
    }

    private void PlaceAttributes(Structure structure)
    {
        Tile originTile = instance.targetedTile.GetComponent<Tile>();
        for (int i = 0; i < structure.attributeTiles.Count; i++)
        {
            Vector2Int rotatedCoordinates = RotateCoordinates(structure.attributeTiles[i]);
            int x = originTile.x + rotatedCoordinates.x;
            int y = originTile.y + rotatedCoordinates.y;

            Tile tile = GetTileAt(x, y);
            if (tile) tile.AddAttribute(structure.attributes[i]);
            structure.attributes[i].transform.position = new Vector3(0, .5f, 0) + PositionOfTile(x, y) + transform.position;
        }
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

    public List<Structure> GetStructures()
    {
        HashSet<Structure> structures = new();
        foreach (Tile tile in tiles) if(tile.structure) structures.Add(tile.structure);
        return structures.ToList<Structure>();
    }
}
