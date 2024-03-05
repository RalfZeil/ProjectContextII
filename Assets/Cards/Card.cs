using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Card : MonoBehaviour
{
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] private float moveSpeed;
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
            relativeMousePosition = transform.position - MouseWorldPosition();
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
            transform.position = MouseWorldPosition() + relativeMousePosition;

            if (TileGrid.IsTileTargeted()) meshRenderer.enabled = false;
            else meshRenderer.enabled = true;
        }
        else if (!isReturning) returnToHand = StartCoroutine(MoveToHandPosition());
    }

    private static Vector3 MouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator MoveToHandPosition()
    {
        isReturning = true;

        Vector3 targetPosition = new Vector3(1.2f * transform.GetSiblingIndex(), 0, 0.001f * transform.GetSiblingIndex());
        while (transform.localPosition != targetPosition)
        {
            targetPosition = new Vector3(1.2f * transform.GetSiblingIndex(), 0, 0.001f * transform.GetSiblingIndex());
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
        meshRenderer.enabled = true;
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
        if (!isQuick) TimeManager.IncrementTurnCount();
        MissionManager.CheckMissions();
        Destroy(gameObject);
    }
}
