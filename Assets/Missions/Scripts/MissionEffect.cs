using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MissionEffect : MonoBehaviour
{
    public CardSettings.CardColor color;
    public int target;

    [HideInInspector] public Mission mission;
    [HideInInspector] public int count = 0;
    [HideInInspector] public float progress = 0;

    public virtual void Setup()
    {

    }

    public virtual void UpdateVisuals(bool isShowingVisuals)
    {

    }

    public virtual void GetReward()
    {

    }
}
