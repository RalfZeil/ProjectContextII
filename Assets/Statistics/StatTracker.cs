using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI natureDisplay, peopleDisplay;
    [SerializeField] private Image natureBar, peopleBar;

    private void OnEnable()
    {
        TileGrid.OnBuild += UpdateStats;
        TileGrid.OnModify += UpdateStats;
    }

    private void OnDisable()
    {
        TileGrid.OnBuild -= UpdateStats;
        TileGrid.OnModify -= UpdateStats;
    }

    public void UpdateStats(Object o)
    {
        int habitatPositive = 0;
        int habitatNegative = 0;
        int peoplePositive = 0;
        int peopleNegative = 0;

        foreach(Structure structure in TileGrid.GetStructures())
        {
            if (structure.isHabitat)
            {
                habitatPositive += structure.ConnectedNature();
                habitatNegative += structure.ConnectedNegative();
            } else if(structure.color == CardSettings.CardColor.People)
            {
                peoplePositive += structure.CountPositive();
                peopleNegative += structure.CountNegative();
            }
        }

        int natureScore = habitatPositive - habitatNegative;
        int peopleScore = peoplePositive - peopleNegative;

        natureDisplay.text = natureScore.ToString();
        peopleDisplay.text = peopleScore.ToString();

        if (habitatPositive + habitatNegative == 0) natureBar.fillAmount = 0.5f;
        else natureBar.fillAmount = habitatPositive / (float) (habitatPositive + habitatNegative);

        if (peoplePositive + peopleNegative == 0) peopleBar.fillAmount = 0.5f;
        else peopleBar.fillAmount = peoplePositive / (float) (peoplePositive + peopleNegative);
    }
}
