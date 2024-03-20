using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableCard : MonoBehaviour
{
    private Card card;
    private void Start()
    {
        card = GetComponent<Card>();
        card.enabled = false;
        GetComponent<CardEffect>().enabled = false;

        transform.SetParent(CameraControl.instance.cardPickParent);
    }

    private void Update()
    {
        transform.localPosition = new Vector3(((transform.GetSiblingIndex() + 1) / (float)(transform.parent.childCount + 1) - 0.5f) * Card.settings.cardPickSpacing, 0, 0);
    }

    private void OnDestroy()
    {
        card.enabled = true;
        GetComponent<CardEffect>().enabled = true;
    }

    private void OnMouseUpAsButton()
    {
        Pick();
    }

    private void Pick()
    {
        foreach (Transform card in transform.parent) if(card != transform) Destroy(card.gameObject);
        transform.SetParent(CameraControl.instance.cardParent);
        Destroy(this);
    }
}
