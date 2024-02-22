using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool isSelected = false;
    private Vector3 relativeMousePosition;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isSelected = true;
            relativeMousePosition = transform.position - MouseWorldPosition();
            Debug.Log("Card Selected");
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(isSelected)
            {
                Transform tile = TargetedTile();
                if (tile) Debug.Log("Tile targeted");
                else Debug.Log("No Target");
            }

            isSelected = false;
        }

        if(isSelected) transform.position = MouseWorldPosition() + relativeMousePosition;
    }

    private static Vector3 MouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Transform TargetedTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tiles"))) return hit.transform;
        return null;
    }
}
