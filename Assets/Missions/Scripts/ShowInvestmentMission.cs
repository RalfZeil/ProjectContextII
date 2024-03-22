using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInvestmentMission : MissionEffect
{
    private void OnEnable()
    {
        Card.OnCardPlay += CheckPlayedCard;
    }

    private void OnDisable()
    {
        Card.OnCardPlay -= CheckPlayedCard;
    }

    private void CheckPlayedCard(CardEffect card)
    {
        if (card.cardType == CardSettings.CardType.Structure) count++;
        else count = 0;
        progress = (float)count / target;

        mission.UpdateDisplay();
        if (count == target) mission.Complete();
    }
}
