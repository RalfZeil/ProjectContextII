using TMPro;
using UnityEngine;

public class StatsDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalPercentage;

    private void Start()
    {
        try
        {
            finalPercentage.text = PlayerPrefs.GetFloat("PC3Score").ToString();
        }
        catch
        {
            finalPercentage.text = "NAN";
        }
        
    }
}
