using UnityEngine;

public class BuildingCell : Cell, IInteractable
{
    public Building currentBuilding;

    private Outline outline;

    #region HIGHTLIGHTING
    public void Highlight()
    {
        outline.enabled = true;
    }

    public void UnHighlight()
    {
        outline.enabled = false;
    }
    #endregion

    public bool Interact()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        UnHighlight();
    }
}
