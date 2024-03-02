using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificationCard : Card
{
    [SerializeField] private GameObject modificationPrefab;
    [SerializeField] private Structure.StructureType targetedType;

    protected override bool CanPlay()
    {
        if (!base.CanPlay()) return false;

        Tile tile = TileGrid.instance.targetedTile;
        return TileGrid.IsTileTargeted() && tile.structure != null && tile.structure.type == targetedType;
    }

    protected override void Play()
    {
        TileGrid.Modify(modificationPrefab);

        base.Play();
    }
}
