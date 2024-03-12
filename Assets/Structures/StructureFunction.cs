using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StructureFunction : MonoBehaviour
{
    [SerializeField] private int baseCooldown;
    [HideInInspector] public int functionTimer = 0;

    private void Update()
    {
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        Structure structure = GetComponent<Structure>();
        structure.timerDisplay.enabled = TileGrid.isShowingTimers;
        structure.timerDisplay.transform.rotation = Camera.main.transform.rotation;
        structure.timerDisplay.text = (FunctionCooldown() - functionTimer).ToString();
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
        return baseCooldown - GetComponent<Structure>().attributeBonus;
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
    }

    public bool CanBeBoosted()
    {
        return functionTimer < FunctionCooldown();
    }
}
