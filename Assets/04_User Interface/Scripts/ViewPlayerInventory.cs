using System.Collections.Generic;
using UnityEngine;

public class ViewPlayerInventory : MonoBehaviour
{
    // Singleton
    public static ViewPlayerInventory Instance;

    public List<ViewCard> cards = new List<ViewCard>();

    [SerializeField] private ViewCard SelectedCard;

    [Header("References")]
    [SerializeField] private GameObject UICardPrefab;


    private void Start()
    {
        Instance = this;

        EventManager<ViewCard>.AddListener(EventType.OnUICardSelect, SelectCard);
    }

    private void OnDestroy()
    {
        EventManager<ViewCard>.RemoveListener(EventType.OnUICardSelect, SelectCard);
    }

    public void AddCard(Card newCard)
    {
        // Spawn new UI Card
        ViewCard newUICard = Instantiate(UICardPrefab, this.transform).GetComponent<ViewCard>();

        // Setup the UI Card
        newUICard.title.text = newCard.displayName;
        newUICard.description.text = newCard.description;
        newUICard.representedCard = newCard;

        cards.Add(newUICard);
    }

    private void SelectCard(ViewCard newCard)
    {
        DeslectCard();
        SelectedCard = newCard;
        SelectedCard.targetScale = new Vector3(2, 2 ,2);
        newCard.transform.SetAsLastSibling();
    }

    private void DeslectCard()
    {
        if(SelectedCard != null)
        {
            SelectedCard.targetScale = Vector3.one;
            SelectedCard = null;
        }
    }
}
