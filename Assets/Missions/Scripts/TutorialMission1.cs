using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMission1 : MissionEffect
{
    [SerializeField] private GameObject headquartersPrefab, nextTutorialMission;
    [SerializeField] private List<string> rewardOptions;

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
        BuildCard buildCard = card.GetComponent<BuildCard>();
        if (buildCard && buildCard.structurePrefab == headquartersPrefab) mission.Complete();
    }

    public override void GetReward()
    {
        foreach (string card in rewardOptions) Card.CreateCard(card, true);
        if(nextTutorialMission) MissionManager.Add(Instantiate(nextTutorialMission).GetComponent<Mission>());
    }
}
