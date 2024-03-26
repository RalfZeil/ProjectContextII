using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private List<Structure> allStructures = new();

    private float scorePercentage;


    private void Start()
    {
        TileGrid.OnBuild += UpdateScore;
    }

    private void OnDestroy()
    {
        TileGrid.OnBuild -= UpdateScore;
    }

    private void UpdateScore(Structure structure)
    {
        allStructures = TileGrid.GetStructures();
        if (allStructures == null) { return; }
        scorePercentage = CalculateNatureCityBalance(allStructures);
    }

    private float CalculateNatureCityBalance(List<Structure> structures)
    {
        float negativeBuilding = 0;
        float natureBuilding = 0;
        float cityBuilding = 0;

        Debug.Log("Given Structures: " + structures.Count);

        foreach (Structure structure in structures)
        {
            if (structure == null) continue;
            foreach (Attribute attribute in structure.attributes)
            {
                if (attribute.type == Attribute.AttributeType.Negative) { negativeBuilding++; }

                else if (attribute.type == Attribute.AttributeType.PositiveNature) { natureBuilding++; }

                else if (attribute.type == Attribute.AttributeType.PositiveCivilisation) { cityBuilding++; }
            }
        }

        Debug.Log("Values: " + negativeBuilding + " " + natureBuilding + " " + cityBuilding);

        float finalPercentage = ((natureBuilding + cityBuilding) / negativeBuilding) * 100;

        PlayerPrefs.SetFloat("PC3Score", finalPercentage);

        return finalPercentage;
    }
}
