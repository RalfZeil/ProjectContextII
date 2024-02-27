using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed, rotationSpeed;
    [SerializeField] private Transform focusPoint, cardParent;

    private float targetRotation = 0;

    public static CameraControl instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CameraMovement();
        CameraRotation();
    }

    private void CameraMovement()
    {
        Vector3 movement = new();
        if (Input.GetKey(KeyCode.W)) movement += new Vector3(0, 0, 1);
        if (Input.GetKey(KeyCode.A)) movement += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.S)) movement += new Vector3(0, 0, -1);
        if (Input.GetKey(KeyCode.D)) movement += new Vector3(1, 0, 0);

        movement = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * movement;

        transform.localPosition += movement.normalized * moveSpeed * Time.deltaTime;
    }

    private void CameraRotation()
    {
        if (targetRotation >= -30 && Input.GetKeyDown(KeyCode.E)) targetRotation -= 90;
        if (targetRotation <= 30 && Input.GetKeyDown(KeyCode.Q)) targetRotation += 90;

        float maxAngle = rotationSpeed * Time.deltaTime;
        float angle = Mathf.Clamp(targetRotation, -1 * maxAngle, maxAngle);
        transform.RotateAround(focusPoint.position, Vector3.up, angle);
        targetRotation -= angle;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetRotation, 0), rotationSpeed * Time.deltaTime);
    }

    public static Transform GetCardParent()
    {
        return instance.cardParent;
    }
}
