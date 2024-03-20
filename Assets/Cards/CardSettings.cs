using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardSettings", order = 1)]
public class CardSettings : ScriptableObject
{
    public float handSpacing, zSpacing, hoverDistance, baseCameraDistance, selectedCameraDistance, moveSpeed, cardPickSpacing;

    public enum CardColor {Nature, People, Industry};
    public enum CardType {Structure, Modification, Action};
    public List<Color> cardColors;
    public List<Material> cardBases, tooltipBackgrounds;
    public List<Sprite> missionBackgrounds;

    public Vector3 modelPosition, modelRotation, modelSize;

    public Material structurePreviewMaterialValid, structurePreviewMaterialInvalid;

    public GameObject buildCardPrefab, modifyCardPrefab;

    public Vector3 tooltipPosition, tooltipOffset;

    public Color GetColor(CardColor color)
    {
        return cardColors[(int) color];
    }

    public Material GetBase(CardColor color)
    {
        return cardBases[(int)color];
    }

    public Material GetTooltipBackground(CardColor color)
    {
        return tooltipBackgrounds[(int)color];
    }

    public Sprite GetMissionBackground(CardColor color)
    {
        return missionBackgrounds[(int)color];
    }
}
