using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float rotateSpeed = 5f;

    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            if (lastMousePosition != Vector3.zero)
            {
                float deltaX = currentMousePosition.x - lastMousePosition.x;
                transform.Rotate(Vector3.up, deltaX * rotateSpeed * Time.deltaTime);
            }
            lastMousePosition = currentMousePosition;
        }
        else
        {
            lastMousePosition = Vector3.zero;
        }
    }
}
