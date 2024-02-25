using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomInCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Slider zoomSlider;

    public float minZoom = 1f;
    public float maxZoom = 10f;
    public float zoomSpeed = 1f;

    private void Start()
    {
        // Initialize slider value to represent current zoom level
        float initialZoom = Mathf.InverseLerp(maxZoom, minZoom, mainCamera.fieldOfView);
        zoomSlider.value = initialZoom;

        // Add listener for slider value change
        zoomSlider.onValueChanged.AddListener(OnZoomSliderChanged);
    }

    private void OnZoomSliderChanged(float value)
    {
        // Invert the value since slider goes from top to bottom
        value = 1 - value;
        
        // Calculate new zoom level based on slider value
        float newZoom = Mathf.Lerp(minZoom, maxZoom, value);

        // Update camera's field of view to zoom in/out
        mainCamera.fieldOfView = newZoom;
    }
}