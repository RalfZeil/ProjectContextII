using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DedicateToPeopleMission : MissionEffect
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
        if (card.cardColor == CardSettings.CardColor.People) count++;
        else count = 0;
        progress = (float)count / target;

        mission.UpdateDisplay();
        if (count == target) mission.Complete();
    }

    public override void GetReward()
    {
        
    }
}
