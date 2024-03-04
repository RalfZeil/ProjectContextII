using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Axis of rotation
    public Vector3 axis = Vector3.up;
    
    // Rotation speed
    public float rotationSpeed = 0.001f;

    void Update()
    {
        // Rotate the object around the axis
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
