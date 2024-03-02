using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int activationThreshold;
    public Vector2Int relativeCoordinates;

    [HideInInspector] public bool isActive = false, isSuppressed = false;

    public Tile tile;
    public AttributeType type;
    public enum AttributeType {Negative, PositiveCivilisation, PositiveNature};

    public void DeleteAttribute()
    {
        if(tile) tile.RemoveAttribute(this);
        Destroy(gameObject);
    }

    public void Activate()
    {
        bool hasChanged = !isActive;

        isActive = true;
        UpdateDisplay();

        if (hasChanged && tile) tile.UpdateAttributes();
    }

    public void Deactivate()
    {
        bool hasChanged = isActive;

        isActive = false;
        UpdateDisplay();

        if (hasChanged && tile) tile.UpdateAttributes();
    }

    public void UpdateDisplay()
    {
        meshRenderer.enabled = isActive && TileGrid.isShowingAttributes;
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void UpdateStatus(int attributeBonus)
    {
        if (isSuppressed) Deactivate();
        else if (type != AttributeType.Negative && attributeBonus > activationThreshold) Activate();
        else if (type == AttributeType.Negative && attributeBonus < activationThreshold) Activate();
        else Deactivate();
    }
}
