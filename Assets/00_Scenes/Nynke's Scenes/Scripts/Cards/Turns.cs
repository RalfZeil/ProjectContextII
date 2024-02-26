using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Turns : MonoBehaviour
{
    public GameObject Mission;
    public GameObject AreYouSureScreen;
    public TextMeshProUGUI Turns_Text;

    IEnumerator Start()
    {
        yield return StartCoroutine(TurnOffObjectAfterDelay());
    }

    IEnumerator TurnOffObjectAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (Mission != null)
            Mission.SetActive(false);
            AreYouSureScreen.SetActive(false);

        Turns_Text.text = "TURN COUNT: 2";
    }
}