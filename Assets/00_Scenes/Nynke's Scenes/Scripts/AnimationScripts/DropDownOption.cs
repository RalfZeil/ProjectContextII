using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownOption : MonoBehaviour

{
    public Dropdown dropdown;
    public GameObject objectToTurnOn;

    void Start()
    {        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);});
    }
    void DropdownValueChanged(Dropdown dropdown)
    {
        if (dropdown.value == 0) 
        {
            objectToTurnOn.SetActive(true);
        }
        else
        {
            objectToTurnOn.SetActive(false);
        }
    }
}
