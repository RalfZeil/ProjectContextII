using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mission : MonoBehaviour
{
    [SerializeField] private CardSettings cardSettings;
    [SerializeField] private TextMeshProUGUI countText, targetText;
    [SerializeField] private Image background, progressBar;
    [SerializeField] private CardSettings.CardColor color;
    [SerializeField] protected int target = 0;

    protected int count = 0;
    protected float progress = 0;
    [HideInInspector] public bool isJustCompleted = false;

    private void Start()
    {
        background.sprite = cardSettings.GetMissionBackground(color);
        UpdateDisplay();
    }

    protected void UpdateDisplay()
    {
        countText.text = count.ToString();
        targetText.text = target.ToString();
        progressBar.fillAmount = progress;
    }

    protected void Complete()
    {
        MissionManager.CompleteMission(this);
    }
    public virtual void GetReward()
    {

    }
}
