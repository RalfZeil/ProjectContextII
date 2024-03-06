using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Card : MonoBehaviour
{
    [SerializeField] protected List<Renderer> renderers;
    [SerializeField] private float moveSpeed, handSpacing, zSpacing;
    [SerializeField] private bool isQuick, isForced;

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

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zSpacing * transform.GetSiblingIndex());

        Vector3 targetPosition = new Vector3(handSpacing * transform.GetSiblingIndex(), 0, zSpacing * transform.GetSiblingIndex());
        while (transform.localPosition != targetPosition)
        {
            targetPosition = new Vector3(handSpacing * transform.GetSiblingIndex(), 0, zSpacing * transform.GetSiblingIndex());
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isReturning = false;
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
        foreach (Card card in GetAllCards()) if (card.isForced) return true;
        return false;
    }

    public static List<Card> GetAllCards()
    {
        return CameraControl.GetCardParent().GetComponentsInChildren<Card>().ToList<Card>();
    }

    public virtual List<Vector2Int> TargetedTiles()
    {
        return new List<Vector2Int>();
    }

    protected virtual bool CanPlay()
    {
        return isForced || (!ForcedCardExists());
    }
    protected virtual void Play()
    {
        wasPlayed = true;
        MissionManager.CheckMissions();
        if (!isQuick) TimeManager.IncrementTurnCount();
        MissionManager.CheckMissions();
        Destroy(gameObject);
    }
}
