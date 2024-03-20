using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateGreenPathwayMission : MissionEffect
{
    private Tile habitatOrigin, targetTile;

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
        List<Structure> habitats = TileGrid.GetHabitats();
        habitatOrigin = habitats[Random.Range(0, habitats.Count)].tiles[0];
        AddRelatedTile(habitatOrigin);

        List<Tile> candidateTiles = TileGrid.GetTilesWithDistanceFromHabitat(habitatOrigin.x, habitatOrigin.y, target);

        if (candidateTiles.Count == 0) mission.Complete();
        else
        {
            targetTile = candidateTiles[Random.Range(0, candidateTiles.Count)];
            AddRelatedTile(targetTile);
        }
    }

    private void CheckMission(Object o)
    {
        if (mission.isCompleted) return;

        count = target - TileGrid.GetDistanceGrid(habitatOrigin.x, habitatOrigin.y, TileGrid.GetBoolNatureGrid())[targetTile.x, targetTile.y];

        mission.UpdateDisplay();
        if (count >= target) mission.Complete();
    }

    public override void GetReward()
    {
        for(int i = 0; i < 3; i++) Card.CreateBuildCard("Flowerbed", true);
    }
}
