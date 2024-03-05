using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private float missionPositionSpacing;
    [SerializeField] private int maxMissionCount;

    public static MissionManager instance;

    private void Awake()
    {
        instance = this;
        UpdateMissionPositions();
    }

    private static void UpdateMissionPositions()
    {
        foreach (Mission mission in GetMissions())
        {
            mission.transform.localPosition = new Vector3(0, -instance.missionPositionSpacing * mission.transform.GetSiblingIndex(), 0);
        }
    }

    public static void Add(Mission mission)
    {
        mission.transform.SetParent(instance.transform);
        mission.transform.localScale = Vector3.one;
        UpdateMissionPositions();
    }

    public static void CheckMissions()
    {
        foreach (Mission mission in GetMissions())
        {
            if (mission.IsCompleted())
            {
                mission.GetReward();
                RemoveMission(mission);
            }
        }
    }

    private static void RemoveMission(Mission mission)
    {
        Destroy(mission.gameObject);
    }

    public static Mission[] GetMissions()
    {
        return instance.GetComponentsInChildren<Mission>();
    }

    public static bool CanFitMoreMissions()
    {
        return GetMissions().Length < instance.maxMissionCount;
    }
}
