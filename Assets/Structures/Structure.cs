//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class Structure : MonoBehaviour
//{
//    [SerializeField] private GameObject associatedCardPrefab;
//    [SerializeField] private int baseCooldown;
//    [SerializeField] private TextMeshPro timerDisplay;
//    public bool hasFunction, isPermanent;
//    public List<Vector2Int> coveredTiles;

//    [HideInInspector] public List<Attribute> attributes = new();
//    [HideInInspector] public List<Tile> tiles = new();
//    [HideInInspector] public List<Modification> modifications = new();
//    [HideInInspector] public int functionTimer = 0, attributeBonus = 0;

//    public enum StructureType {Civilisation, Nature, Industry};
//    public StructureType type;

//    private void Awake()
//    {
//        foreach (Attribute attribute in GetComponentsInChildren<Attribute>())
//        {
//            attributes.Add(attribute);
//            attribute.structure = this;
//        }
//    }

//    private void Start()
//    {

//    }

//    private void Update()
//    {
//        if (hasFunction) UpdateTimerDisplay();
//    }

//    private void UpdateTimerDisplay()
//    {
//        timerDisplay.transform.rotation = Camera.main.transform.rotation;
//        timerDisplay.text = (FunctionCooldown() - functionTimer).ToString();
//    }

//    public void ReturnToHand()
//    {
//        CreateAssociatedCard();

//        foreach (Attribute attribute in attributes) attribute.DeleteAttribute();
//        foreach (Modification modification in modifications) modification.Delete();
//        Destroy(gameObject);
//    }

//    private void CreateAssociatedCard()
//    {
//        if (!associatedCardPrefab) return;

//        Transform card = Instantiate(associatedCardPrefab).transform;
//        card.position = transform.position;
//    }

//    protected virtual bool CanActivate()
//    {
//        return false;
//    }

//    protected virtual void Activate()
//    {

//    }

//    private int FunctionCooldown()
//    {
//        return baseCooldown - attributeBonus;
//    }

//    public void TakeTurn(int amount = 1)
//    {
//        if (!hasFunction) return;

//        int functionCooldown = FunctionCooldown();
//        functionTimer += amount;
//        if (functionTimer > functionCooldown) functionTimer = functionCooldown;
//        if (functionTimer == functionCooldown && CanActivate())
//        {
//            Activate();
//            functionTimer = 0;
//        }
//    }

//    public bool CanBeBoosted()
//    {
//        return hasFunction && functionTimer < FunctionCooldown();
//    }

//    public void UpdateAttributeBonus()
//    {
//        attributeBonus = 0;

//        foreach (Tile tile in tiles) foreach (Attribute attribute in tile.attributes) if(attribute.isActive)
//        {
//            if (attribute.type == Attribute.AttributeType.Negative) attributeBonus--;
//            else if (type == StructureType.Civilisation && attribute.type == Attribute.AttributeType.PositiveCivilisation) attributeBonus++;
//        }

//        if (type == StructureType.Nature) attributeBonus += NatureAttributeBonus();

//        foreach (Attribute attribute in GetAllAttributes()) attribute.UpdateStatus();
//    }

//    private int NatureAttributeBonus()
//    {
//        int[,] natureGrid = TileGrid.GetNatureGrid();
//        bool[,] visitedGrid = new bool[natureGrid.GetLength(0), natureGrid.GetLength(1)];

//        int areaCount = TileGrid.countArea(OriginTile().x, OriginTile().y, natureGrid, visitedGrid);
//        return (int) Mathf.Floor(Mathf.Sqrt(areaCount));
//    }

//    public Tile OriginTile()
//    {
//        return tiles[0];
//    }

//    public void UpdateAttributeLayering()
//    {
//        UnsuppressAttributes();

//        List<Attribute> accumulativeAttributes = new List<Attribute>(attributes);
//        foreach (Modification modification in modifications)
//        {
//            if (modification.isReplacing) foreach (Attribute modAttribute in modification.attributes)
//            {
//                foreach (Attribute attribute in accumulativeAttributes) if (attribute.tile == modAttribute.tile) attribute.isSuppressed = true;
//            }
//            accumulativeAttributes.AddRange(modification.attributes);
//        }

//        foreach (Attribute attribute in accumulativeAttributes) attribute.UpdateStatus();
//    }

//    private void UnsuppressAttributes()
//    {
//        foreach (Attribute attribute in attributes) attribute.isSuppressed = false;
//        foreach (Modification modification in modifications) foreach (Attribute attribute in modification.attributes) attribute.isSuppressed = false;
//    }

//    public List<Attribute> GetAllAttributes()
//    {
//        List<Attribute> accumulativeAttributes = new List<Attribute>(attributes);
//        foreach (Modification modification in modifications) accumulativeAttributes.AddRange(modification.attributes);
//        return accumulativeAttributes;
//    }
//}
