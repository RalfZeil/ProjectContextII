using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public List<Vector2Int> coveredTiles, attributeTiles;

    public List<Attribute> attributes = new();

    void Awake()
    {
        foreach (Attribute attribute in GetComponentsInChildren<Attribute>()) attributes.Add(attribute);
    }

    void Update()
    {
        
    }
}
