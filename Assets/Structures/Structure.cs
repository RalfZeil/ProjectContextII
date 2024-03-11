using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Structure : MonoBehaviour
{
    [SerializeField] private int baseCooldown;
    [SerializeField] private TextMeshPro timerDisplay;
    [SerializeField] private Tooltip tooltip;
    public bool hasFunction, isPermanent, isReplayable;
    public List<Vector2Int> coveredTiles;
    public string title, description;

    [HideInInspector] public List<Attribute> attributes = new();
    [HideInInspector] public List<Tile> tiles = new();
    [HideInInspector] public List<Modification> modifications = new();
    [HideInInspector] public int functionTimer = 0, attributeBonus = 0;

    public GameObject model, attributeObject;
    public CardSettings.CardColor color;

    private void Awake()
    {
        foreach (Attribute attribute in GetComponentsInChildren<Attribute>())
        {
            attributes.Add(attribute);
            attribute.structure = this;
        }
    }

    private void Start()
    {
        tooltip.Initialize(title, description, CardSettings.CardType.Structure.ToString(), color, CardSettings.CardType.Structure);
    }

    private void Update()
    {
        if (hasFunction) UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        timerDisplay.enabled = TileGrid.isShowingTimers;
        timerDisplay.transform.rotation = Camera.main.transform.rotation;
        timerDisplay.text = (FunctionCooldown() - functionTimer).ToString();
    }

    public void ReturnToHand()
    {
        if (isReplayable) CreateAssociatedCard();

        foreach (Tile tile in tiles) tile.structure = null;
        foreach (Attribute attribute in attributes) attribute.DeleteAttribute();
        foreach (Modification modification in modifications) modification.Delete();
        Destroy(tooltip.gameObject);
        Destroy(gameObject);
    }

    private void CreateAssociatedCard()
    {
        Transform card = Card.CreateBuildCard(title).transform;
        card.position = transform.position;
    }

    protected virtual bool CanActivate()
    {
        return false;
    }

    protected virtual void Activate()
    {

    }

    public void Select()
    {
        foreach (Attribute attribute in attributes) attribute.SetHighlight(true);
        tooltip.UpdateRendering(true);
    }

    public void Deselect()
    {
        foreach (Attribute attribute in attributes) attribute.SetHighlight(false);
        tooltip.UpdateRendering(false);
    }

    private int FunctionCooldown()
    {
        return baseCooldown - attributeBonus;
    }

    public void TakeTurn(int amount = 1)
    {
        if (!hasFunction) return;

        int functionCooldown = FunctionCooldown();
        functionTimer += amount;
        if (functionTimer > functionCooldown) functionTimer = functionCooldown;
        if (functionTimer == functionCooldown && CanActivate())
        {
            Activate();
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
            else if (color == CardSettings.CardColor.People && attribute.type == Attribute.AttributeType.PositiveCivilisation) attributeBonus++;
        }

        if (color == CardSettings.CardColor.Nature) attributeBonus += NatureAttributeBonus();

        foreach (Attribute attribute in GetAllAttributes()) attribute.UpdateStatus();
    }

    private int NatureAttributeBonus()
    {
        int[,] natureGrid = TileGrid.GetNatureGrid();
        bool[,] visitedGrid = new bool[natureGrid.GetLength(0), natureGrid.GetLength(1)];

        int areaCount = TileGrid.countArea(OriginTile().x, OriginTile().y, natureGrid, visitedGrid);
        return (int) Mathf.Floor(Mathf.Sqrt(areaCount));
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
