using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPlacement : MonoBehaviour
{
    public GameObject objectToPlace;
    public Transform gridPlane;
    public float gridSize = 1f;
    public Button cardButton;
    public Button yesButton;
    public Button noButton;
    public GameObject AreYouSureScreen; 
    public GameObject ProgressionScreen;

    private bool isPlacing = false;
    private bool isDragging = false;
    private Vector3 mouseOffset;

    void Start()
    {
        cardButton.onClick.AddListener(TogglePlacement);
        yesButton.onClick.AddListener(ConfirmPlacement);
        noButton.onClick.AddListener(CancelPlacement);
    }

    void Update()
    {
        if (isPlacing)
        {
            if (!isDragging)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceObject();
                }
                else
                {
                    MoveObjectWithMouse();
                }
            }
        }
    }

    void TogglePlacement()
    {
        isPlacing = !isPlacing;

        if (isPlacing)
        {
            objectToPlace.SetActive(true);
            Vector3 mousePos = GetMouseWorldPosition();
            mouseOffset = objectToPlace.transform.position - mousePos;

            cardButton.gameObject.SetActive(false);
        }
        else
        {
            ShowConfirmationDialog();
        }
    }

    void ShowConfirmationDialog()
    {
        AreYouSureScreen.SetActive(true);
    }

    void ConfirmPlacement()
    {
        cardButton.gameObject.SetActive(false);
        ProgressionScreen.SetActive(true);
    }

    void CancelPlacement()
    {
        AreYouSureScreen.SetActive(false); 
        cardButton.gameObject.SetActive(true); 
        isPlacing = false;
    }
    void MoveObjectWithMouse()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        float snappedX = Mathf.Round((mousePos.x + mouseOffset.x) / gridSize) * gridSize;
        float snappedZ = Mathf.Round((mousePos.z + mouseOffset.z) / gridSize) * gridSize;
        objectToPlace.transform.position = new Vector3(snappedX, objectToPlace.transform.position.y, snappedZ);

        if (Input.GetMouseButton(0))
        {
            isDragging = true;
        }
        else
        {
            isDragging = false;
        }
    }

    void PlaceObject()
    {
        if (isPlacing)
        {
            isPlacing = false;
            Debug.Log("Object Placed at: " + objectToPlace.transform.position);
            ShowConfirmationDialog();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, gridPlane.position);
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.gray;
            float width = 10f;
            float height = 10f;
            for (float x = 0; x <= width; x += gridSize)
            {
                Vector3 start = gridPlane.position + new Vector3(x, 0f, 0f);
                Vector3 end = start + new Vector3(0f, 0f, height);
                Gizmos.DrawLine(start, end);
            }
            for (float z = 0; z <= height; z += gridSize)
            {
                Vector3 start = gridPlane.position + new Vector3(0f, 0f, z);
                Vector3 end = start + new Vector3(width, 0f, 0f);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}