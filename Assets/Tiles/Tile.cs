using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material baseMaterial, selectedMaterial, nopeMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform attributeDisplay;
    [SerializeField] private float attributeDisplaySpread;

     public Structure structure;
    [HideInInspector] public Modification modification;
     public List<Attribute> attributes = new();

    [HideInInspector] public int x, y;

    private void OnMouseOver()
    {
        
    }

    public void Select()
    {
        meshRenderer.material = selectedMaterial;
        if (structure) structure.Select();
        if (modification) modification.Select();
    }

    public void Deselect()
    {
        meshRenderer.material = baseMaterial;
        if (structure) structure.Deselect();
        if (modification) modification.Deselect();
    }

    public void AddAttribute (Attribute attribute)
    {
        attributes.Add(attribute);
        attribute.tile = this;
        attribute.transform.parent = attributeDisplay;
        attribute.transform.localRotation = Quaternion.identity;
        attribute.transform.localPosition = Vector3.zero;
    }

    public void SortAttributes()
    {
        attributes = attributes.OrderBy(x => x.GetSortPriority()).ToList();

        foreach (Attribute attribute in attributes) attribute.transform.SetParent(null);
        foreach (Attribute attribute in attributes) attribute.transform.SetParent(attributeDisplay);
    }

    public void PositionAttributes()
    {
        List<Attribute> visibleAttributes = GetVisibleAttributes();

        for (int i = 0; i < visibleAttributes.Count; i++)
        {
            Attribute attribute = visibleAttributes[i];
            float attributePosition = (i - (visibleAttributes.Count-1)/2f);
            attributePosition *= attributeDisplaySpread;
            attributePosition /= visibleAttributes.Count;
            attribute.transform.localPosition = new Vector3(attributePosition, 0, -attributePosition);
        }
    }

    private List<Attribute> GetVisibleAttributes()
    {
        List<Attribute> visibleAttributes = new();
        foreach (Attribute attribute in attributes) if (attribute.IsVisible()) visibleAttributes.Add(attribute);
        return visibleAttributes;
    }

    public void RemoveAttribute(Attribute attribute)
    {
        attributes.Remove(attribute);
    }

    public void Replace()
    {
        if (structure == null) return;

        structure.ReturnToHand();
        structure = null;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateAttributeDisplay();
    }

    private void UpdateAttributeDisplay()
    {
        attributeDisplay.rotation = Camera.main.transform.rotation;
    }
}
