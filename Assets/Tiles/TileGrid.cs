using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int gridWidth, gridHeight;

    private Tile[,] tiles;

    private void Start()
    {
        tiles = new Tile[gridWidth, gridHeight];

        for(int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Transform newTile = Instantiate(tilePrefab).transform;
                newTile.parent = transform;
                newTile.localPosition = new Vector3(i, 0, j);
                tiles[i, j] = newTile.GetComponent<Tile>();
            }
        }
    }

    private void Update()
    {
        
    }
}
