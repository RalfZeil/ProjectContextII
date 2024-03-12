using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardStructure : StructureFunction
{
    [SerializeField] private string cardName;
    [SerializeField] private CardSettings.CardType createdCardType;
    [SerializeField] private GameObject createdCardPrefab;
    private Transform createdCard;
    protected override bool CanActivate()
    {
        return (createdCard == null || createdCard.GetComponent<Card>().wasPlayed);
    }

    protected override void Activate()
    {
        if (createdCardType == CardSettings.CardType.Structure) createdCard = Card.CreateBuildCard(cardName).transform;
        else if (createdCardType == CardSettings.CardType.Modification) createdCard = Card.CreateModifyCard(cardName).transform;
        else createdCard = Instantiate(createdCardPrefab).transform;
        createdCard.position = transform.position;
    }
}
