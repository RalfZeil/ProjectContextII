using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardSettings", order = 1)]
public class CardSettings : ScriptableObject
{
    public float maxHandSpacing, maxHandWidth, zSpacing, hoverDistance, baseCameraDistance, selectedCameraDistance, moveSpeed, cardPickSpacing;
    public int cardRewardOptions;
    public List<string> cardRewardsNature, cardRewardsPeople, cardRewardsIndustry;

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

    public void SpawnCardReward(CardColor color, int amount)
    {
        List<string> cardNames = cardRewardsIndustry;
        if (color == CardColor.Nature) cardNames = cardRewardsNature;
        else if (color == CardColor.People) cardNames = cardRewardsPeople;

        amount = Mathf.Min(amount, cardNames.Count);
        List<string> pickedCards = cardNames.OrderBy(x => Random.value).Take(amount).ToList();

        foreach (string card in pickedCards) Card.CreateCard(card, true);
    }
}
