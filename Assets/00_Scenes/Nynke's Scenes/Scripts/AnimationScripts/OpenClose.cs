using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{
    public Animator animator;
    private bool isOpen = false;
    public MouseController mousecontroller;

    public void ToggleOpenClose()
    {
        isOpen = !isOpen; // Toggle the isOpen state
        if (isOpen)
        {
            animator.SetFloat("Open", 1f);
            if(mousecontroller != null)
                mousecontroller.enabled = false; 
        }
        else
        {
            animator.SetFloat("Open", 0f);
            if(mousecontroller != null)
                mousecontroller.enabled = true;
        }
    }
}
