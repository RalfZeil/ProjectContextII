using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler // This class implements drag and drop functionality for UI elements, including checking for overlap with the delete image.
{
    public GameObject deleteImage;
    public GameObject Mission;

    private RectTransform _draggingObject;
    private Vector2 _originalPosition;
    private CanvasGroup _canvasGroup;

    public float overlapThreshold = 1f;

    private void Start()
    {
        _draggingObject = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(Mission.transform.position, deleteImage.transform.position);

        if (distance < overlapThreshold)
        {
            Debug.Log("Objects are overlapping!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _originalPosition = _draggingObject.anchoredPosition;
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _draggingObject.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = true;
        }
        _draggingObject.anchoredPosition = _originalPosition;
    }
}
