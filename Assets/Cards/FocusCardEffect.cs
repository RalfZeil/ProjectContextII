using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCardEffect : CardEffect
{
    [SerializeField] private int boostAmount;

    public override bool CanPlay()
    {
        foreach (Tile tile in TileGrid.GetTargetedTiles())
        {
            if (tile.structure && tile.structure.CanBeBoosted()) return true;
        }

        return false;
    }

    public override void Play()
    {
        foreach (Tile tile in TileGrid.GetTargetedTiles())
        {
            if (tile.structure) tile.structure.TakeTurn(boostAmount);
        }
    }
}
