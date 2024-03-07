using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCard : Card
{
    [SerializeField] private GameObject structurePrefab;

    protected override bool CanPlay()
    {
        if (!base.CanPlay()) return false;

        return TileGrid.IsTileTargeted() && TileGrid.IsValidPlacement();
    }

    protected override void Play()
    {
        TileGrid.Build(structurePrefab);

        base.Play();
    }

    public override List<Vector2Int> TargetedTiles()
    {
        return structurePrefab.GetComponent<Structure>().coveredTiles;
    }
}
