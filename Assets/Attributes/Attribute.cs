using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int activationThreshold;

    [HideInInspector] public bool isActive = true;

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
        Debug.Log("activate");
        if (isActive) return;

        meshRenderer.enabled = true;
        isActive = true;
        if (tile) tile.UpdateAttributes();
    }

    public void Deactivate()
    {
        Debug.Log("deactivate");
        if (!isActive) return;

        meshRenderer.enabled = false;
        isActive = false;
        if (tile) tile.UpdateAttributes();
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void UpdateStatus(int attributeBonus)
    {
        Debug.Log("update status: " + attributeBonus);
        if (type != AttributeType.Negative && attributeBonus > activationThreshold) Activate();
        else if (type == AttributeType.Negative && attributeBonus < activationThreshold) Activate();
        else Deactivate();
    }
}
