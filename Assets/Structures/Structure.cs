using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Structure : MonoBehaviour
{
    [SerializeField] private GameObject associatedCardPrefab, createdCardPrefab;
    [SerializeField] private int baseCooldown;
    [SerializeField] private TextMeshPro timerDisplay;
    public bool hasFunction, isPermanent;
    public List<Vector2Int> coveredTiles, attributeTiles;

    [HideInInspector] public List<Attribute> attributes = new();

    private int functionTimer;
    private Transform createdCard;

    private void Awake()
    {
        foreach (Attribute attribute in GetComponentsInChildren<Attribute>()) attributes.Add(attribute);

        functionTimer = baseCooldown;
    }

    private void Update()
    {
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        timerDisplay.transform.rotation = Camera.main.transform.rotation;
        timerDisplay.text = functionTimer.ToString();
        timerDisplay.enabled = hasFunction;
    }

    public void ReturnToHand()
    {
        Transform card = Instantiate(associatedCardPrefab).transform;
        card.position = transform.position;

        foreach (Attribute attribute in attributes) Destroy(attribute.gameObject);
        Destroy(gameObject);
    }

    public void TakeTurn()
    {
        if (!hasFunction) return;

        functionTimer--;
        if (functionTimer < 0) functionTimer = 0;
        if (functionTimer == 0 && (createdCard == null || createdCard.GetComponent<Card>().wasPlayed))
        {
            createdCard = Instantiate(createdCardPrefab).transform;
            createdCard.position = transform.position;
            functionTimer = baseCooldown;
        }
    }

    public void BoostTurn(int amount)
    {
        if (!hasFunction) return;

        functionTimer -= amount;
        if (functionTimer < 0) functionTimer = 0;
        if (functionTimer == 0 && createdCard == null)
        {
            createdCard = Instantiate(createdCardPrefab).transform;
            createdCard.position = transform.position;
            functionTimer = baseCooldown;
        }
    }

    public bool CanBeBoosted()
    {
        return hasFunction && functionTimer > 0;
    }
}
