using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutIntoActionMission : MissionEffect
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
        target = Mathf.Max(CameraControl.GetCardParent().childCount - target, target);
    }

    private void CheckPlayedCard(CardEffect card)
    {
        count = CameraControl.GetCardParent().childCount;
        progress = 0f;

        mission.UpdateDisplay();
        if (count <= target) mission.Complete();
    }
}
