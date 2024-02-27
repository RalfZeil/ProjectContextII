using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float moveSpeed;

    private bool isSelected = false;
    private Vector3 relativeMousePosition;

    private Coroutine returnToHand;
    private bool isReturning = false;

    public static Card heldCard = null;

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

    private void TryPlay()
    {
        if (TileGrid.IsTileTargeted() && TileGrid.IsValidPlacement()) Play();

        heldCard = null;
        isSelected = false;
        meshRenderer.enabled = true;
    }

    private void Play()
    {
        TileGrid.Build(structurePrefab);
        TimeManager.IncrementTurnCount();
        Destroy(gameObject);
    }

    private static Vector3 MouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator MoveToHandPosition()
    {
        isReturning = true;

        Vector3 targetPosition = new Vector3(1.2f * transform.GetSiblingIndex(), 0, 0.1f * transform.GetSiblingIndex());
        while (transform.localPosition != targetPosition)
        {
            targetPosition = new Vector3(1.2f * transform.GetSiblingIndex(), 0, 0.1f * transform.GetSiblingIndex());
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isReturning = false;
    }

    public List<Vector2Int> TargetedTiles()
    {
        return structurePrefab.GetComponent<Structure>().coveredTiles;
    }
}
