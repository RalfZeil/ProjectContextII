using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private CardEffect cardEffect;
    [SerializeField] private CardSettings cardSettings;
    [SerializeField] private TextMeshPro titleText, typeText, descriptionText;
    [SerializeField] private MeshRenderer baseCard;
    [SerializeField] private Transform graphicsObject, previewObject;

    private List<Renderer> renderers = new();
    private List<Attribute> previewAttributes = new();
    protected bool isSelected = false;
    private Vector3 relativeMousePosition;

    private Coroutine returnToHand;
    private bool isReturning = false;

    [HideInInspector] public bool wasPlayed = false, isHovered = false;

    public static Card heldCard = null;
    private static CardSettings settings;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isSelected = true;
            relativeMousePosition = transform.localPosition - MouseLocalPosition();
            heldCard = this;

            if (returnToHand != null) StopCoroutine(returnToHand);
            isReturning = false;
        }

        isHovered = true;
    }

    private void OnMouseExit()
    {
        isHovered = false;
    }

    private void Start()
    {
        if (!Card.settings) Card.settings = cardSettings;

        transform.parent = CameraControl.GetCardParent();
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

        descriptionText.text = cardEffect.cardDescription;
        titleText.text = cardEffect.cardTitle;
        typeText.text = cardEffect.cardType.ToString();
        typeText.color = cardSettings.GetColor(cardEffect.cardColor);
        baseCard.material = cardSettings.GetBase(cardEffect.cardColor);

        CreateModel();
        CreatePreviewModel();
        CreatePreviewAttributes();
        SetRenderers();
    }

    private void CreateModel()
    {
        GameObject model = Instantiate(cardEffect.GetModel());
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Renderer renderer in model.transform.GetComponentsInChildren<Renderer>()) bounds.Encapsulate(renderer.bounds);

        model.transform.parent = graphicsObject;
        model.transform.localPosition = cardSettings.modelPosition;
        model.transform.localRotation = Quaternion.Euler(cardSettings.modelRotation);
        float size = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z, 0.001f);
        model.transform.localScale = cardSettings.modelSize / size;
    }

    private void CreatePreviewModel()
    {
        GameObject model = Instantiate(cardEffect.GetModel());
        foreach (Renderer renderer in model.transform.GetComponentsInChildren<Renderer>()) renderer.enabled = false;

        model.transform.parent = previewObject;
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;
        model.transform.localScale = Vector3.one;
    }

    private void CreatePreviewAttributes()
    {
        GameObject attributes = Instantiate(cardEffect.GetAttributes());
        foreach (Attribute attribute in attributes.GetComponentsInChildren<Attribute>())
        {
            if (attribute.isAlwaysActive)
            {
                previewAttributes.Add(attribute);
                attribute.SetHighlight(true);
            }
            else attribute.DeleteAttribute();
        }
    }

    private void SetRenderers()
    {
        foreach (Renderer renderer in graphicsObject.GetComponentsInChildren<Renderer>())
        {
            renderers.Add(renderer);
            renderer.gameObject.layer = LayerMask.NameToLayer("UI");
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    private void Update()
    {
        if (isSelected && Input.GetMouseButtonUp(0)) TryPlay();

        if (isSelected)
        {
            Vector3 position = MouseLocalPosition() + relativeMousePosition;
            position.z = cardSettings.selectedCameraDistance;
            transform.localPosition = position;

            UpdateRendering(TileGrid.IsTileTargeted());
        }
        else if (!isReturning) returnToHand = StartCoroutine(MoveToHandPosition());
    }

    private void UpdateRendering(bool isInPreviewMode)
    {
        foreach (Renderer renderer in renderers) renderer.enabled = !isInPreviewMode;
        foreach (Renderer renderer in previewObject.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = isInPreviewMode;
            if (isInPreviewMode)
            {
                renderer.material = CanPlay() ? cardSettings.structurePreviewMaterialValid : cardSettings.structurePreviewMaterialInvalid;

                previewObject.position = (new Vector3(0, .3f, 0)) + TileGrid.instance.targetedTile.transform.position;
                previewObject.rotation = Quaternion.Euler(0, TileGrid.targetRotation, 0);
            }
        }

        foreach (Attribute attribute in previewAttributes)
        {
            if (isInPreviewMode) attribute.Activate();
            else attribute.Deactivate();
            if (attribute.tile)
            {
                attribute.tile.RemoveAttribute(attribute);
                attribute.tile = null;
            }
        }

        if (isInPreviewMode)
        {
            TileGrid.PlaceAttributes(previewAttributes, TileGrid.instance.targetedTile);
            foreach (Attribute attribute in previewAttributes) if (attribute.tile)
                {
                    attribute.tile.SortAttributes();
                    attribute.tile.PositionAttributes();
                }
        }
    }

    private Vector3 MouseLocalPosition()
    {
        return transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private IEnumerator MoveToHandPosition()
    {
        isReturning = true;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, GetLocalZ());

        Vector3 targetPosition = new Vector3(cardSettings.handSpacing * transform.GetSiblingIndex(), HoverOffset(), GetLocalZ());
        while (transform.localPosition != targetPosition)
        {
            targetPosition = new Vector3(cardSettings.handSpacing * transform.GetSiblingIndex(), HoverOffset(), GetLocalZ());
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, cardSettings.moveSpeed * Time.deltaTime);
            yield return null;
        }

        isReturning = false;
    }

    private float HoverOffset()
    {
        return (isHovered && !CameraControl.IsRotating()) ? cardSettings.hoverDistance : 0;
    }

    private float GetLocalZ()
    {
        return cardSettings.baseCameraDistance - cardSettings.zSpacing * transform.GetSiblingIndex();
    }

    private void TryPlay()
    {
        if (CanPlay()) Play();

        heldCard = null;
        isSelected = false;
        UpdateRendering(false);
    }

    public static bool ForcedCardExists()
    {
        foreach (Card card in GetAllCards()) if (card.cardEffect.isForced) return true;
        return false;
    }

    public static List<Card> GetAllCards()
    {
        return CameraControl.GetCardParent().GetComponentsInChildren<Card>().ToList<Card>();
    }

    public List<Vector2Int> TargetedTiles()
    {
        return cardEffect.TargetedTiles();
    }

    private bool CanPlay()
    {
        return (cardEffect.isForced || (!ForcedCardExists())) && cardEffect.CanPlay();
    }
    private void Play()
    {
        wasPlayed = true;
        MissionManager.CheckMissions();
        if (!cardEffect.isQuick) TimeManager.IncrementTurnCount();
        MissionManager.CheckMissions();

        foreach (Attribute attribute in previewAttributes) attribute.DeleteAttribute();
        Destroy(gameObject);

        cardEffect.Play();
    }

    public static GameObject CreateBuildCard(string structureName)
    {
        GameObject card = Instantiate(settings.buildCardPrefab);
        card.GetComponent<BuildCard>().Initialize((GameObject)Resources.Load("StructurePrefabs/" + structureName));

        return card;
    }

    public static GameObject CreateModifyCard(string modificationName)
    {
        GameObject card = Instantiate(settings.modifyCardPrefab);
        card.GetComponent<ModificationCard>().Initialize((GameObject)Resources.Load("ModificationPrefabs/" + modificationName));

        return card;
    }
}
