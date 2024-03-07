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
}
