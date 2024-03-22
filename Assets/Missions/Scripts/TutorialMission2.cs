using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMission2 : MissionEffect
{
    [SerializeField] private GameObject nextTutorialMission;
    [SerializeField] private List<string> rewardOptions;

    private void OnEnable()
    {
        CameraControl.OnCameraMove += CheckMission;
    }

    private void OnDisable()
    {
        CameraControl.OnCameraMove -= CheckMission;
    }

    private void CheckMission()
    {
        mission.Complete();
    }

    public override void GetReward()
    {
        foreach (string card in rewardOptions) Card.CreateCard(card, true);
        if(nextTutorialMission) MissionManager.Add(Instantiate(nextTutorialMission).GetComponent<Mission>());
    }
}
