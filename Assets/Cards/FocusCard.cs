using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCard : Card
{
    [SerializeField] private int boostAmount;

    protected override bool CanPlay()
    {
        if (!base.CanPlay()) return false;

        foreach (Tile tile in TileGrid.GetTargetedTiles())
        {
            if (tile.structure && tile.structure.CanBeBoosted()) return true;
        }

        return false;
    }

    protected override void Play()
    {
        foreach (Tile tile in TileGrid.GetTargetedTiles())
        {
            if (tile.structure) tile.structure.BoostTurn(boostAmount);
        }

        base.Play();
    }
}
