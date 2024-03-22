using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocateCardEffect : CardEffect
{
    [SerializeField] private GameObject model;

    public override bool CanPlay()
    {
        if (!TileGrid.IsTileTargeted()) return false;

        Structure structure = TileGrid.instance.targetedTile.structure;
        return structure && structure.isReplayable && !structure.isPermanent;
    }

    public override void Play()
    {
        TileGrid.instance.targetedTile.structure.ReturnToHand();
    }

    public override GameObject GetModel()
    {
        return model;
    }
}
