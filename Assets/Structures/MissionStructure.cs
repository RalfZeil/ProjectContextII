//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class MissionStructure : Structure
//{
//    [SerializeField] private List<GameObject> createdMissionPrefabs;
//    private Mission createdMission;
//    protected override bool CanActivate()
//    {
//        return (createdMission == null || createdMission.isJustCompleted) && MissionManager.CanFitMoreMissions();
//    }

//    protected override void Activate()
//    {
//        createdMission = Instantiate(createdMissionPrefabs[Random.Range(0, createdMissionPrefabs.Count)]).GetComponent<Mission>();
//        MissionManager.Add(createdMission);
//    }
//}
