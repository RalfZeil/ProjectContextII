using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnDisplay;
    [SerializeField] private int startingYear;

    public static int turnCount = 0;
    private static TimeManager instance;

    private enum months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    } ;

    private void Start()
    {
        instance = this;

        UpdateTurnDisplay();
    }

    private void UpdateTurnDisplay()
    {
        string currentYear = (startingYear + turnCount / 12).ToString();
        string currentMonth = ((months)(turnCount % 12)).ToString();
        turnDisplay.text = currentYear + "\n" + currentMonth;
    }

    public static void IncrementTurnCount()
    {
        turnCount++;
        instance.UpdateTurnDisplay();

        foreach (Structure structure in TileGrid.GetStructures()) structure.TakeTurn();
    }
}
