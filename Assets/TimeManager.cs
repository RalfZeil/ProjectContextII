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

    private void OnEnable()
    {
        Card.OnCardPlay += CheckCardPlayed;
    }

    private void OnDisable()
    {
        Card.OnCardPlay -= CheckCardPlayed;
    }

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

    private void CheckCardPlayed(CardEffect card)
    {
        if (!card.isQuick) IncrementTurnCount();
    }

    private void IncrementTurnCount()
    {
        turnCount++;
        UpdateTurnDisplay();

        foreach (StructureFunction function in TileGrid.GetStructureFunctions()) function.TakeTurn();
    }
}
