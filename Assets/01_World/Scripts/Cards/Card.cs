using UnityEngine;

public class Card : ScriptableObject
{
    [Header("General Settings")]
    public string displayName;
    public string description;

    public virtual void Interact(BuildingCell cellToInteractWith)
    {

    }
}
