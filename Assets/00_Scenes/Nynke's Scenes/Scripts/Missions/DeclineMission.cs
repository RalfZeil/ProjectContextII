using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeclineMission : MonoBehaviour
{
    public Button DeleteButton;
    public Button AreYouSureButton;
    public GameObject objectToDelete;

    private bool DeleteButtonClicked = false;

    void Start()
    {
        DeleteButton.onClick.AddListener(OnDeleteButtonClick);
        AreYouSureButton.onClick.AddListener(OnAreYouSureButtonClick);
    }

    void OnDeleteButtonClick()
    {
        DeleteButtonClicked = true;
    }

    void OnAreYouSureButtonClick()
    {
        if (DeleteButtonClicked)
        {
            if (objectToDelete != null)
            {
                Destroy(objectToDelete);
                Debug.Log("Object deleted!");
            }
            else
            {
                Debug.LogWarning("No object assigned to delete!");
            }
        }
    }
}