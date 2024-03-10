using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    public string cardTitle, cardDescription;
    public CardSettings.CardType cardType;
    public CardSettings.CardColor cardColor;
    public bool isQuick, isForced;

    public virtual bool CanPlay()
    {
        return true;
    }

    public virtual void Play()
    {
        
    }

    public virtual List<Vector2Int> TargetedTiles()
    {
        return new List<Vector2Int>();
    }

    public virtual GameObject GetModel()
    {
        return new GameObject("Card Model");
    }
}
