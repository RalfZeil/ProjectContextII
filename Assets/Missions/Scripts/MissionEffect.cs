using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MissionEffect : MonoBehaviour
{
    public CardSettings.CardColor color;
    public int target;

    [HideInInspector] public Mission mission;
    [HideInInspector] public int count = 0;
    [HideInInspector] public float progress = 0;

    protected List<Tile> relatedTiles = new();
    protected List<Transform> visuals = new();

    public virtual void Setup()
    {

    }

    public void UpdateVisuals(bool isShowingVisuals)
    {
        foreach (Transform t in visuals) foreach (Renderer renderer in t.GetComponentsInChildren<Renderer>()) renderer.enabled = isShowingVisuals;
    }

    protected void AddRelatedTile(Tile tile)
    {
        relatedTiles.Add(tile);
        Transform indicator = Instantiate(mission.indicatorPrefab).transform;
        indicator.SetParent(tile.transform);
        indicator.position = tile.transform.position;
        indicator.localRotation = Quaternion.identity;
        visuals.Add(indicator);
    }

    public void DestroyVisuals()
    {
        foreach (Transform t in visuals) Destroy(t.gameObject);
    }

    public virtual void GetReward()
    {
        Card.settings.SpawnCardReward(color, Card.settings.cardRewardOptions);
    }
}
