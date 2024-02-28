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
    [HideInInspector] public List<Tile> tiles = new();

    public int functionTimer = 0, attributeBonus = 0;
    private Transform createdCard;

    public enum StructureType {Civilisation, Nature};
    public StructureType type;

    private void Awake()
    {
        foreach (Attribute attribute in GetComponentsInChildren<Attribute>()) attributes.Add(attribute);
        timerDisplay.enabled = hasFunction;
    }

    private void Start()
    {
        UpdateAttributeBonus();
    }

    private void Update()
    {
        if (hasFunction) UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        timerDisplay.transform.rotation = Camera.main.transform.rotation;
        timerDisplay.text = (FunctionCooldown() - functionTimer).ToString();
    }

    public void ReturnToHand()
    {
        Transform card = Instantiate(associatedCardPrefab).transform;
        card.position = transform.position;

        foreach (Attribute attribute in attributes) attribute.DeleteAttribute();
        Destroy(gameObject);
    }

    public void TakeTurn()
    {
        if (!hasFunction) return;

        int functionCooldown = FunctionCooldown();
        functionTimer++;
        if (functionTimer > functionCooldown) functionTimer = functionCooldown;
        if (functionTimer == functionCooldown && (createdCard == null || createdCard.GetComponent<Card>().wasPlayed))
        {
            createdCard = Instantiate(createdCardPrefab).transform;
            createdCard.position = transform.position;
            functionTimer = 0;
        }
    }

    private int FunctionCooldown()
    {
        return baseCooldown - attributeBonus;
    }

    public void BoostTurn(int amount)
    {
        if (!hasFunction) return;

        int functionCooldown = FunctionCooldown();
        functionTimer += amount;
        if (functionTimer > functionCooldown) functionTimer = functionCooldown;
        if (functionTimer == functionCooldown && (createdCard == null || createdCard.GetComponent<Card>().wasPlayed))
        {
            createdCard = Instantiate(createdCardPrefab).transform;
            createdCard.position = transform.position;
            functionTimer = 0;
        }
    }

    public bool CanBeBoosted()
    {
        return hasFunction && functionTimer < FunctionCooldown();
    }

    public void UpdateAttributeBonus()
    {
        attributeBonus = 0;

        foreach (Tile tile in tiles) foreach (Attribute attribute in tile.attributes) if(attribute.isActive)
        {
            if (attribute.type == Attribute.AttributeType.Negative) attributeBonus--;
            else if (type == StructureType.Civilisation && attribute.type == Attribute.AttributeType.PositiveCivilisation) attributeBonus++;
            else if (type == StructureType.Nature && attribute.type == Attribute.AttributeType.PositiveNature) attributeBonus++;
        }

        foreach (Attribute attribute in attributes) attribute.UpdateStatus(attributeBonus);
    }
}
