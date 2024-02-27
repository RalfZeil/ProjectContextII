using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material baseMaterial, selectedMaterial, nopeMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    public Structure structure;
    private List<Attribute> attributes = new();

    public int x, y;

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

    public bool IsFree()
    {
        return structure == null;
    }

    public void AddAttribute (Attribute attribute)
    {
        attributes.Add(attribute);
        attribute.transform.parent = transform;
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }
}
