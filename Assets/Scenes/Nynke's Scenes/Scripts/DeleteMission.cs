using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMission : MonoBehaviour
{
    public RectTransform MissionButton;
    public RectTransform DeleteButton;

    void Update()
    {
        // Convert the RectTransform positions to screen space
        Vector3 missionButtonScreenPosition = Camera.main.WorldToScreenPoint(MissionButton.position);
        Vector3 deleteButtonScreenPosition = Camera.main.WorldToScreenPoint(DeleteButton.position);

        // Check if MissionButton is above DeleteButton
        if (missionButtonScreenPosition.y > deleteButtonScreenPosition.y)
        {
            Debug.Log("Mission is above Delete Button");
            StartCoroutine(TurnOffMissionButton());
        }
        else
        {
            Debug.Log("Mission is not above Delete Button");
        }
    }

    IEnumerator TurnOffMissionButton()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        MissionButton.gameObject.SetActive(false); // Turn off the MissionButton
    }
}