using UnityEngine;

public enum ActionType
{
    Remove = 0,
    OtherAction = 1
}

[CreateAssetMenu()]
public class ActionCard : Card
{
    [Header("Gameplay Settings")]
    public ActionType actionType;

    [Header("References")]
    public GameObject prefab;

    
}
