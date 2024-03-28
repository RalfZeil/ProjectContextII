using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalTooltip : MonoBehaviour
{
    [SerializeField] private Renderer[] negativeIcons, positiveIcons;
    [SerializeField] private int breakpoint, interval;
    [SerializeField] private Vector3 startPosition;

    private void Start()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = startPosition;
    }

    public void UpdateRendering(bool isVisible, int value)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) renderer.enabled = isVisible;

        if(isVisible)
        {
            for(int i = 0; i < negativeIcons.Length; i++)
            {
                negativeIcons[i].enabled = breakpoint - i * interval > value;
                positiveIcons[i].enabled = breakpoint + i * interval < value;
            }
        }
    }
}
