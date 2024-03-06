using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewCard : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI description;
    [SerializeField] private Button cardButton;

    public Card representedCard;

    private void Start()
    {
        cardButton.onClick.AddListener(() => EventManager<ViewCard>.RaiseEvent(EventType.OnUICardSelect, this));
    }

    private void OnDestroy()
    {
        cardButton.onClick.RemoveListener(() => EventManager<ViewCard>.RaiseEvent(EventType.OnUICardSelect, this));
    }

    private void Update()
    {
        float lerpSpeed = 0.1f;

        int cardIndex = ViewPlayerInventory.Instance.cards.IndexOf(this);
        int cardCount = ViewPlayerInventory.Instance.cards.Count;

        float alignResult = cardIndex / (cardCount - 1.0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(alignResult * Screen.width, transform.localPosition.y, transform.localPosition.z), lerpSpeed);
    }
}