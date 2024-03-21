using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpandIndustryMission : MissionEffect
{
    private void OnEnable()
    {
        TileGrid.OnBuild += CheckMission;
    }

    private void OnDisable()
    {
        TileGrid.OnBuild -= CheckMission;
    }

    public override void Setup()
    {
        List<Tile> candidateTiles = new();
        foreach (Tile tile in TileGrid.instance.tiles) if (!IsIndustrial(tile) && (!tile.structure || !tile.structure.isPermanent)) candidateTiles.Add(tile);

        target = Mathf.Min(target, candidateTiles.Count);
        List<Tile> selectedTiles = candidateTiles.OrderBy(x => Random.value).Take(target).ToList();

        foreach (Tile tile in selectedTiles) AddRelatedTile(tile);
    }

    private bool IsIndustrial(Tile tile)
    {
        return tile.structure && tile.structure.color == CardSettings.CardColor.Industry;
    }

    private void CheckMission(Structure structure)
    {
        count = 0;
        foreach (Tile tile in relatedTiles) if (IsIndustrial(tile)) count++;
        progress = (float)count / target;

        mission.UpdateDisplay();
        if (count >= target) mission.Complete();
    }
}
