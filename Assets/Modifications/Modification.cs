using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Modification : MonoBehaviour
{
    [SerializeField] private Tooltip tooltip;
    public bool isReplacing;
    public GameObject model, attributeObject;
    public string title, description;
    public CardSettings.CardColor color;
    [HideInInspector] public List<Attribute> attributes = new();
    [HideInInspector] public Tile tile;
    [HideInInspector] public Structure structure;

    private void Awake()
    {
        structure = TileGrid.instance.targetedTile.structure;

        foreach (Attribute attribute in GetComponentsInChildren<Attribute>())
        {
            attributes.Add(attribute);
            attribute.structure = structure;
        }
    }

    private void Start()
    {
        tooltip.Initialize(title, description, CardSettings.CardType.Structure.ToString(), color, CardSettings.CardType.Modification);
    }

    private void Update()
    {
        
    }

    public void Remove()
    {
        structure.modifications.Remove(this);
        Delete();
    }

    public void Delete()
    {
        foreach (Attribute attribute in attributes) attribute.DeleteAttribute();
        tile.modification = null;
        structure.UpdateAttributeLayering();
        Destroy(gameObject);
    }

    public void Select()
    {
        foreach (Attribute attribute in attributes) attribute.SetHighlight(true);
        tooltip.UpdateRendering(true);
    }

    public void Deselect()
    {
        foreach (Attribute attribute in attributes) attribute.SetHighlight(false);
        tooltip.UpdateRendering(false);
    }
}
