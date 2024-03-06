using UnityEngine;

[CreateAssetMenu()]
public class Building : Card
{
    [Header("Gameplay Settings")]
    public bool isNatureBuilding;

    [Header("References")]
    public GameObject prefab;

    public override void Interact(BuildingCell cellToInteractWith)
    {
        cellToInteractWith.CurrentBuilding = this;
    }
}
