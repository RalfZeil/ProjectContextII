using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardStructure : StructureFunction
{
    [SerializeField] private string cardName;
    private Transform createdCard;
    protected override bool CanActivate()
    {
        return (createdCard == null || createdCard.GetComponent<Card>().wasPlayed);
    }

    protected override void Activate()
    {
        createdCard = Card.CreateCard(cardName).transform;
        createdCard.position = transform.position;
    }
}
