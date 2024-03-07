using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI description;
    [SerializeField] private Button cardButton;

    [Header("Card UI Settings")]
    [SerializeField] private float lerpSpeed = 0.1f;
    [SerializeField] private float padding = 200f;

    public Card representedCard;

    [HideInInspector] public Vector3 targetScale = new Vector3(1, 1, 1);

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
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        int cardIndex = ViewPlayerInventory.Instance.cards.IndexOf(this);
        int cardCount = ViewPlayerInventory.Instance.cards.Count;

        float alignResult = cardIndex / (cardCount - 1.0f);

        transform.position = Vector3.Lerp(
            transform.position, // Original Position
            new Vector3((alignResult * (Screen.width - padding * 2)) + padding, transform.position.y, transform.position.z), // New Position
            lerpSpeed
        );

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed);
    }
}