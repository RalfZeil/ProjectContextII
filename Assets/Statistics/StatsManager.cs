using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private List<Structure> allStructures = new();

    private void Update()
    {
        allStructures = TileGrid.GetStructures();

        if (allStructures == null) { return; }

        Debug.Log(CalculateNatureCityBalance(allStructures));
    }

    private float CalculateNatureCityBalance(List<Structure> structures)
    {
        int negativeBuilding = 0;
        int natureBuilding = 0;
        int cityBuilding = 0;

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

        float finalPercentage = ((natureBuilding + cityBuilding) / negativeBuilding) * 100;

        return finalPercentage;
    }
}
