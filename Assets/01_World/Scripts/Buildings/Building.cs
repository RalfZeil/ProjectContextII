using UnityEngine;

[CreateAssetMenu(fileName = "Building")]
public class Building : ScriptableObject
{
    [Header("Settings")]
    public string displayName;
    public string description;
    public bool isNatureBuilding;

    [Header("References")]
    public GameObject prefab;
}
