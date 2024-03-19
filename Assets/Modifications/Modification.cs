//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class Modification : MonoBehaviour
//{
//    public bool isReplacing;
//    [HideInInspector] public List<Attribute> attributes = new();
//    [HideInInspector] public Tile tile;
//    [HideInInspector] public Structure structure;

//    private void Awake()
//    {
//        //structure = TileGrid.instance.targetedTile.structure;
//        foreach (Attribute attribute in GetComponentsInChildren<Attribute>())
//        {
//            attributes.Add(attribute);
//            attribute.structure = structure;
//        }
//    }

//    private void Start()
//    {
        
//    }

//    private void Update()
//    {
        
//    }

//    public void Remove()
//    {
//        structure.modifications.Remove(this);
//        Delete();
//    }

//    public void Delete()
//    {
//        foreach (Attribute attribute in attributes) attribute.DeleteAttribute();
//        tile.modification = null;
//        structure.UpdateAttributeLayering();
//        Destroy(gameObject);
//    }
//}
