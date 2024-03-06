using UnityEngine;

public class BuildingCell : Cell, IInteractable
{
    private Building currentBuilding;

    public Building CurrentBuilding 
    {
        set 
        {
            currentBuilding = value;

            // Remove currend child
            Destroy(transform.GetChild(0).gameObject);

            // Instantiate new child for visual
            if (currentBuilding != null)
            {
                Instantiate(currentBuilding.prefab, this.transform);
            }
        }
        get 
        {
            return currentBuilding;
        }
    }

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
        //BuildStructure();
        return true;
    }

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        UnHighlight();
    }

    public bool InteractWithCard(Card card)
    {
        card.Interact(this);
        return true;
    }
}
