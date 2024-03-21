using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCardEffect : CardEffect
{
    [SerializeField] private GameObject model;
    [SerializeField] private int boostAmount;

    public override bool CanPlay()
    {
        foreach (Tile tile in TileGrid.GetTargetedTiles()) if (tile.structure)
        {
            StructureFunction function = tile.structure.GetComponent<StructureFunction>();
            if (function && function.CanBeBoosted()) return true;
        }

        return false;
    }

    public override void Play()
    {
        foreach (Tile tile in TileGrid.GetTargetedTiles()) if (tile.structure)
        {
            StructureFunction function = tile.structure.GetComponent<StructureFunction>();
            if (function) function.TakeTurn(boostAmount);
        }
    }

    public override GameObject GetModel()
    {
        return model;
    }
}
