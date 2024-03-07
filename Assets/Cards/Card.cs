using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private CardEffect cardEffect;
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private CardSettings cardSettings;
    [SerializeField] private TextMeshPro titleText, typeText, descriptionText;
    [SerializeField] private MeshRenderer baseCard;

    protected bool isSelected = false;
    private Vector3 relativeMousePosition;

    private Coroutine returnToHand;
    private bool isReturning = false;

    [HideInInspector] public bool wasPlayed = false;
    [HideInInspector] public static Card heldCard = null;

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
    }

    private void Start()
    {
        transform.parent = CameraControl.GetCardParent();
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

        descriptionText.text = cardEffect.cardDescription;
        titleText.text = cardEffect.cardTitle;
        typeText.text = cardEffect.cardType.ToString();
        typeText.color = cardSettings.GetColor(cardEffect.cardColor);
        baseCard.material = cardSettings.GetBase(cardEffect.cardColor);
    }

    private void Update()
    {
        if (isSelected && Input.GetMouseButtonUp(0)) TryPlay();

        if (isSelected)
        {
            transform.localPosition = MouseLocalPosition() + relativeMousePosition;

            SetVisibility(!TileGrid.IsTileTargeted());
        }
        else if (!isReturning) returnToHand = StartCoroutine(MoveToHandPosition());
    }

    private void SetVisibility(bool visibility)
    {
        foreach (Renderer renderer in renderers) renderer.enabled = visibility;
    }

    private Vector3 MouseLocalPosition()
    {
        return transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private IEnumerator MoveToHandPosition()
    {
        isReturning = true;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, GetLocalZ());

        Vector3 targetPosition = new Vector3(cardSettings.handSpacing * transform.GetSiblingIndex(), 0, GetLocalZ());
        while (transform.localPosition != targetPosition)
        {
            targetPosition = new Vector3(cardSettings.handSpacing * transform.GetSiblingIndex(), 0, GetLocalZ());
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, cardSettings.moveSpeed * Time.deltaTime);
            yield return null;
        }

        isReturning = false;
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
        SetVisibility(true);
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
        cardEffect.Play();

        wasPlayed = true;
        MissionManager.CheckMissions();
        if (!cardEffect.isQuick) TimeManager.IncrementTurnCount();
        MissionManager.CheckMissions();
        Destroy(gameObject);
    }
}
