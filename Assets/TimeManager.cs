using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnDisplay;
    [SerializeField] private int startingYear, turnLimit;
    [SerializeField] private Canvas endgameScreen;

    public static int turnCount = 0;
    private static TimeManager instance;

    [HideInInspector] public delegate void EndGameAction();
    public static event EndGameAction OnGameEnd;

    public bool isGameOver = false;

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

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateTurnDisplay()
    {
        string currentYear = (startingYear + turnCount / 12).ToString();
        string currentMonth = ((months)(turnCount % 12)).ToString();
        turnDisplay.text = currentYear + " - " + currentMonth;
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


        if(turnCount >= turnLimit)
        {
            endgameScreen.enabled = true;
            foreach (Card card in Card.GetAllCards()) Destroy(card.gameObject);
            foreach (Mission mission in MissionManager.GetMissions()) Destroy(mission.gameObject);

            OnGameEnd?.Invoke();
            isGameOver = true;
        }
    }
}
