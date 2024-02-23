using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownOption : MonoBehaviour

{
    // Reference to the dropdown UI element
    public Dropdown dropdown;

    // Reference to the object you want to turn on
    public GameObject objectToTurnOn;

    void Start()
    {
        // Subscribe to the dropdown's OnValueChanged event
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    // Method to handle dropdown value change
    void DropdownValueChanged(Dropdown dropdown)
    {
        // Check if option 1 is selected
        if (dropdown.value == 0) // Adjust the value according to your dropdown options
        {
            // Turn on the object
            objectToTurnOn.SetActive(true);
        }
        else
        {
            // Turn off the object (optional)
            objectToTurnOn.SetActive(false);
        }
    }
}
