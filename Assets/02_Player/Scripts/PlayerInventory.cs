using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Singleton
    public static PlayerInventory Instance;

    public List<Card> heldCards;

    public Card selectedCard;

    private void Start()
    {
        Instance = this;
        heldCards = new List<Card>();
    }

    public void AddCardToInventory(Card newCard)
    {
        heldCards.Append(newCard);
    }

    public void SelectCard(Card card)
    {
        selectedCard = card;
    }
}
