using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerInputActions _actions;

    // Start is called before the first frame update
    void Start()
    {
        _actions = new PlayerInputActions();
        _actions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
