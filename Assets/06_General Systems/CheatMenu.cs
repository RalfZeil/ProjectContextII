using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public Card TestSpawnedCard;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Add Card"))
        {
            ViewPlayerInventory.Instance.AddCard(TestSpawnedCard);
        }
    }
}
