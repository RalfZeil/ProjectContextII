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
    public List<Vector2Int> coveredTiles;

    [HideInInspector] public List<Attribute> attributes = new();
    [HideInInspector] public List<Tile> tiles = new();
    [HideInInspector] public List<Modification> modifications = new();

    public int functionTimer = 0, attributeBonus = 0;
    private Transform createdCard;

    public enum StructureType {Civilisation, Nature, Industry};
    public StructureType type;

    private void Awake()
    {
        foreach (Attribute attribute in GetComponentsInChildren<Attribute>())
        {
            attributes.Add(attribute);
            attribute.structure = this;
        }
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
        foreach (Modification modification in modifications) modification.Delete();
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
        }

        if (type == StructureType.Nature) attributeBonus += NatureAttributeBonus();

        foreach (Attribute attribute in attributes) attribute.UpdateStatus();
    }

    private int NatureAttributeBonus()
    {
        int[,] natureGrid = TileGrid.GetNatureGrid();
        bool[,] visitedGrid = new bool[natureGrid.GetLength(0), natureGrid.GetLength(1)];

        return TileGrid.countArea(OriginTile().x, OriginTile().y, natureGrid, visitedGrid);
    }

    public Tile OriginTile()
    {
        return tiles[0];
    }

    public void UpdateAttributeLayering()
    {
        UnsuppressAttributes();

        List<Attribute> accumulativeAttributes = new List<Attribute>(attributes);
        foreach (Modification modification in modifications)
        {
            if (modification.isReplacing) foreach (Attribute modAttribute in modification.attributes)
            {
                foreach (Attribute attribute in accumulativeAttributes) if (attribute.tile == modAttribute.tile) attribute.isSuppressed = true;
            }
            accumulativeAttributes.AddRange(modification.attributes);
        }

        foreach (Attribute attribute in accumulativeAttributes) attribute.UpdateStatus();
    }

    private void UnsuppressAttributes()
    {
        foreach (Attribute attribute in attributes) attribute.isSuppressed = false;
        foreach (Modification modification in modifications) foreach (Attribute attribute in modification.attributes) attribute.isSuppressed = false;
    }

    public List<Attribute> GetAllAttributes()
    {
        List<Attribute> accumulativeAttributes = new List<Attribute>(attributes);
        foreach (Modification modification in modifications) accumulativeAttributes.AddRange(modification.attributes);
        return accumulativeAttributes;
    }
}
