using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;

    public void OnPointerEnter(PointerEventData d)
    {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData d)
    {
        tooltip.SetActive(false);
    }
}
