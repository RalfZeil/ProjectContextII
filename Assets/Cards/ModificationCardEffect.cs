using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificationCard : CardEffect
{
    [SerializeField] private GameObject modificationPrefab;
    [SerializeField] private CardSettings.CardColor targetedColor;

    public override bool CanPlay()
    {
        Tile tile = TileGrid.instance.targetedTile;
        return TileGrid.IsTileTargeted() && tile.structure != null && tile.structure.color == targetedColor;
    }

    public override void Play()
    {
        TileGrid.Modify(modificationPrefab);
    }

    public override GameObject GetModel()
    {
        return modificationPrefab.GetComponent<Modification>().model;
    }

    public override GameObject GetAttributes()
    {
        return modificationPrefab.GetComponent<Modification>().attributeObject;
    }

    public void Initialize(GameObject prefab)
    {
        modificationPrefab = prefab;
        Modification modification = modificationPrefab.GetComponent<Modification>();
        cardTitle = modification.title;
        cardDescription = modification.description;
        cardColor = modification.color;
        targetedColor = modification.color;
    }
}
