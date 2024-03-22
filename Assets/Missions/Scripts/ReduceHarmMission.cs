using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReduceHarmMission : MissionEffect
{
    Tile targetedTile;

    private void OnEnable()
    {
        TileGrid.OnBuild += CheckMission;
        TileGrid.OnModify += CheckMission;
    }

    private void OnDisable()
    {
        TileGrid.OnBuild -= CheckMission;
        TileGrid.OnModify -= CheckMission;
    }

    public override void Setup()
    {
        List<Tile> candidateTiles = new();
        foreach (Tile tile in TileGrid.instance.tiles) if (HarmCount(tile) == target) candidateTiles.Add(tile);

        targetedTile = candidateTiles[Random.Range(0, candidateTiles.Count)];
        AddRelatedTile(targetedTile);
    }

    private int HarmCount(Tile tile)
    {
        int count = 0;
        foreach (Attribute attribute in tile.attributes) if (attribute.isActive && attribute.type == Attribute.AttributeType.Negative) count++;
        return count;
    }

    private void CheckMission(Object o)
    {
        count = HarmCount(targetedTile);
        progress = 1 - (float) count / target;

        mission.UpdateDisplay();
        if (count == 0) mission.Complete();
    }
}
