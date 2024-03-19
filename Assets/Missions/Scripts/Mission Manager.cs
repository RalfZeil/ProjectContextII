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
            mission.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -instance.missionPositionSpacing * mission.transform.GetSiblingIndex());
        }
    }

    public static void Add(Mission mission)
    {
        mission.transform.SetParent(instance.transform);
        mission.transform.localScale = Vector3.one;
        UpdateMissionPositions();
    }

    public static void CompleteMission(Mission mission)
    {
        mission.missionEffect.GetReward();
        mission.isJustCompleted = true;
        RemoveMission(mission);
    }

    private static void RemoveMission(Mission mission)
    {
        mission.transform.SetParent(null);
        mission.missionEffect.DestroyVisuals();
        Destroy(mission.gameObject);
        UpdateMissionPositions();
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
