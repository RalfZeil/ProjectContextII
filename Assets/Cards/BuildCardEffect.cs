using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCard : CardEffect
{
    [SerializeField] private GameObject structurePrefab;

    public override bool CanPlay()
    {
        return TileGrid.IsTileTargeted() && TileGrid.IsValidPlacement();
    }

    public override void Play()
    {
        TileGrid.Build(structurePrefab, TileGrid.GetTargetedTiles());
    }

    public override List<Vector2Int> TargetedTiles()
    {
        return structurePrefab.GetComponent<Structure>().coveredTiles;
    }

    public override GameObject GetModel()
    {
        return structurePrefab.GetComponent<Structure>().model;
    }

    public override GameObject GetAttributes()
    {
        return structurePrefab.GetComponent<Structure>().attributeObject;
    }

    public void Initialize(GameObject prefab)
    {
        structurePrefab = prefab;
        Structure structure = structurePrefab.GetComponent<Structure>();
        cardTitle = structure.title;
        cardDescription = structure.description;
        cardColor = structure.color;
    }
}
