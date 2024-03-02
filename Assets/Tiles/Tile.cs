using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material baseMaterial, selectedMaterial, nopeMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform attributeDisplay;
    [SerializeField] private float attributeDisplaySpread;

    [HideInInspector] public Structure structure;
    [HideInInspector] public Modification modification;
    [HideInInspector] public List<Attribute> attributes = new();

    [HideInInspector] public int x, y;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            meshRenderer.material = selectedMaterial;
        }
    }

    public void Select()
    {
        meshRenderer.material = selectedMaterial;
    }

    public void Deselect()
    {
        meshRenderer.material = baseMaterial;
    }

    public void AddAttribute (Attribute attribute)
    {
        attributes.Add(attribute);
        attribute.tile = this;
        attribute.transform.parent = attributeDisplay;
        attribute.transform.localRotation = Quaternion.identity;
        UpdateAttributes();
    }

    public void UpdateAttributes()
    {
        PositionAttributes();

        if (structure) structure.UpdateAttributeBonus();
    }

    private void PositionAttributes()
    {
        List<Attribute> activeAttributes = GetActiveAttributes();

        for (int i = 0; i < activeAttributes.Count; i++)
        {
            Attribute attribute = activeAttributes[i];
            float attributePosition = (i - (activeAttributes.Count-1)/2f);
            attributePosition *= attributeDisplaySpread;
            attributePosition /= activeAttributes.Count;
            attribute.transform.localPosition = new Vector3(attributePosition, 0, 0);
        }
    }

    private List<Attribute> GetActiveAttributes()
    {
        List<Attribute> activeAttributes = new();
        foreach (Attribute attribute in attributes) if (attribute.isActive) activeAttributes.Add(attribute);
        return activeAttributes;
    }

    public void RemoveAttribute(Attribute attribute)
    {
        attributes.Remove(attribute);
        UpdateAttributes();
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
