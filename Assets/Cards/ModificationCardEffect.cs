using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificationCard : CardEffect
{
    [SerializeField] private GameObject modificationPrefab;
    [SerializeField] private Structure.StructureType targetedType;

    public override bool CanPlay()
    {
        Tile tile = TileGrid.instance.targetedTile;
        return TileGrid.IsTileTargeted() && tile.structure != null && tile.structure.type == targetedType;
    }

    public override void Play()
    {
        TileGrid.Modify(modificationPrefab);
    }
}
