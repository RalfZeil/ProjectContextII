using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer, highlightRenderer;
    [SerializeField] private int activationThreshold;
    [SerializeField] private bool isAlwaysActive, isHighlighted;
    public Vector2Int relativeCoordinates;

    [HideInInspector] public bool isActive = false, isSuppressed = false;
    [HideInInspector] public Structure structure;
    [HideInInspector] public Tile tile;

    public AttributeType type;
    public enum AttributeType {Negative, PositiveCivilisation, PositiveNature};

    public void DeleteAttribute()
    {
        if(tile) tile.RemoveAttribute(this);
        Destroy(gameObject);
    }

    public void Activate()
    {
        if (!isActive) TileGrid.attributeHasChangedSinceUpdate = true;

        isActive = true;
        UpdateDisplay();
    }

    public void Deactivate()
    {
        if (isActive) TileGrid.attributeHasChangedSinceUpdate = true;

        isActive = false;
        UpdateDisplay();
    }

    public void SetHighlight(bool highlight)
    {
        isHighlighted = highlight;
        UpdateDisplay();
        if (tile) tile.PositionAttributes();
    }

    public void UpdateDisplay()
    {
        meshRenderer.enabled = IsVisible();
        highlightRenderer.enabled = isHighlighted && IsVisible();
    }

    public bool IsVisible()
    {
        return isActive && (isHighlighted || TileGrid.isShowingAttributes[(int)type]);
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void UpdateStatus()
    {
        if (isSuppressed || tile == null) Deactivate();
        else if (isAlwaysActive) Activate();
        else if (type != AttributeType.Negative && structure.attributeBonus > activationThreshold) Activate();
        else if (type == AttributeType.Negative && structure.attributeBonus < activationThreshold) Activate();
        else Deactivate();
    }

    public int GetSortPriority()
    {
        if (type == AttributeType.Negative) return 1;
        if (type == AttributeType.PositiveNature) return 2;
        return 3;
    }
}
