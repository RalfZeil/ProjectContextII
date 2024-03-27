using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public bool rotateX = true;
    public bool rotateY = false;
    public bool rotateZ = false;

    public float rotationSpeed = 30f;

    // Update is called once per frame
    void Update()
    {
        // Determine which axes to rotate based on inspector settings
        float xRotation = rotateX ? rotationSpeed * Time.deltaTime : 0f;
        float yRotation = rotateY ? rotationSpeed * Time.deltaTime : 0f;
        float zRotation = rotateZ ? rotationSpeed * Time.deltaTime : 0f;

        // Apply rotation
        transform.Rotate(new Vector3(xRotation, yRotation, zRotation));
    }
}
