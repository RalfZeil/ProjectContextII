using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StructureFunction : MonoBehaviour
{
    [SerializeField] private int baseCooldown;
    [HideInInspector] public int functionTimer = 0;
    private Structure structure;

    private void Awake()
    {
        structure = GetComponent<Structure>();
    }

    private void Update()
    {
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        structure.timerDisplay.rotation = Camera.main.transform.rotation;
        structure.timerDisplay.GetComponentInChildren<TextMeshPro>().text = (FunctionCooldown() - functionTimer).ToString();
        foreach (Renderer renderer in structure.timerDisplay.GetComponentsInChildren<Renderer>()) renderer.enabled = TileGrid.isShowingTimers;
        foreach (Image renderer in structure.timerDisplay.GetComponentsInChildren<Image>()) renderer.enabled = TileGrid.isShowingTimers;
    }

    protected virtual bool CanActivate()
    {
        return false;
    }

    protected virtual void Activate()
    {

    }

    private int FunctionCooldown()
    {
        if (structure.isUnaffectedByAttributes) return baseCooldown;
        return baseCooldown - structure.attributeBonus;
    }

    public void TakeTurn(int amount = 1)
    {
        int functionCooldown = FunctionCooldown();
        functionTimer += amount;
        if (functionTimer > functionCooldown) functionTimer = functionCooldown;
        if (functionTimer == functionCooldown && CanActivate())
        {
            Activate();
            functionTimer = 0;
        }

        float progress = 0;
        if (FunctionCooldown() > 0) progress = (FunctionCooldown() - functionTimer) / (float)FunctionCooldown();
        structure.timerProgressBar.fillAmount = progress;
    }

    public bool CanBeBoosted()
    {
        return functionTimer < FunctionCooldown();
    }
}
