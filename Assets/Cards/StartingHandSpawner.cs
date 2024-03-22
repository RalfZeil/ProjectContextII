using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingHandSpawner : MonoBehaviour
{
    [SerializeField] private List<string> cards;

    private void Start()
    {
        foreach (string card in cards) Card.CreateCard(card);
    }
}
