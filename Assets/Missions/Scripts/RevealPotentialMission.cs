using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPotentialMission : MissionEffect
{
    private void OnEnable()
    {
        Card.OnCardPlay += CheckPlayedCard;
        Card.OnCardCreation += CheckPlayedCard;
    }

    private void OnDisable()
    {
        Card.OnCardPlay -= CheckPlayedCard;
        Card.OnCardCreation -= CheckPlayedCard;
    }

    public override void Setup()
    {
        target = CameraControl.GetCardParent().childCount + target;
    }

    private void CheckPlayedCard(CardEffect card)
    {
        count = CameraControl.GetCardParent().childCount;
        progress = (float)count / target;

        mission.UpdateDisplay();
        if (count >= target) mission.Complete();
    }
}
