using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardSettings", order = 1)]
public class CardSettings : ScriptableObject
{
    public float handSpacing, zSpacing, baseCameraDistance, moveSpeed;

    public enum CardColor {Nature, People, Industry};
    public enum CardType {Structure, Modification, Action};
    public List<Color> cardColors;
    public List<Material> cardBases;

    public Color GetColor(CardColor color)
    {
        return cardColors[(int) color];
    }

    public Material GetBase(CardColor color)
    {
        return cardBases[(int)color];
    }
}
