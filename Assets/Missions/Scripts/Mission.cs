using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Mission : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CardSettings cardSettings;
    [SerializeField] private TextMeshProUGUI countText, targetText;
    [SerializeField] private Image background, progressBar, highlight;

    private bool isPermaSelected = false;
    [HideInInspector] public MissionEffect missionEffect;
    [HideInInspector] public bool isJustCompleted = false;

    private void Awake()
    {
        missionEffect = GetComponent<MissionEffect>();
        missionEffect.mission = this;
    }

    private void Start()
    {
        background.sprite = cardSettings.GetMissionBackground(missionEffect.color);
        missionEffect.Setup();
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        countText.text = missionEffect.count.ToString();
        targetText.text = missionEffect.target.ToString();
        progressBar.fillAmount = missionEffect.progress;
    }

    public void Complete()
    {
        MissionManager.CompleteMission(this);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        UpdateSelection(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        UpdateSelection(false);
    }

    public void OnPointerClick(PointerEventData data)
    {
        isPermaSelected = !isPermaSelected;
    }

    private void UpdateSelection(bool isSelected)
    {
        if (isPermaSelected) isSelected = true;
        highlight.enabled = isSelected;

        missionEffect.UpdateVisuals(isSelected);
    }
}
