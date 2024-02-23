using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrid : MonoBehaviour
{
    public Transform gridObject; // Reference to the object with the grid component
    public float moveSpeed = 5f; // Movement speed of the object

    private Vector3 targetPosition;

    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

        // Get the closest point on the grid to the mouse position
        Vector3Int closestGridPosition = gridObject.GetComponent<Grid>().WorldToCell(mousePosition);
        Vector3 targetWorldPosition = gridObject.GetComponent<Grid>().CellToWorld(closestGridPosition);

        // Update the target position
        targetPosition = new Vector3(targetWorldPosition.x, transform.position.y, targetWorldPosition.z);

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}