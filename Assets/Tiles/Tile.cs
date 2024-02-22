using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Tile Selected");
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }
}
