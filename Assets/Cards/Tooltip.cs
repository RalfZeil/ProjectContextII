using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private CardSettings cardSettings;
    [SerializeField] private TextMeshPro titleText, typeText, descriptionText;
    [SerializeField] private MeshRenderer background;

    private List<Renderer> renderers = new();
    private CardSettings.CardType type;

    public void Initialize(string title, string description, string type, CardSettings.CardColor color, CardSettings.CardType tooltipType)
    {
        transform.parent = CameraControl.GetTooltipParent();
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        titleText.text = title;
        descriptionText.text = description;
        typeText.text = type;
        typeText.color = cardSettings.GetColor(color);
        background.material = cardSettings.GetTooltipBackground(color);
        this.type = tooltipType;

        SetRenderers();
    }

    private void SetRenderers()
    {
        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>()) renderers.Add(renderer);
    }

    public void UpdateRendering(bool isVisible)
    {
        foreach (Renderer renderer in renderers)renderer.enabled = isVisible;

        if (isVisible)
        {
            transform.position = TileGrid.instance.targetedTile.transform.position;
            Vector3 localPosition = transform.localPosition;
            localPosition.z = 0;
            localPosition += cardSettings.tooltipPosition;
            localPosition += GetOffset();

            transform.localPosition = localPosition;
        }
    }

    private Vector3 GetOffset()
    {
        Tile tile = TileGrid.instance.targetedTile;

        if (tile.structure && tile.modification) {
            if (type == CardSettings.CardType.Structure) return -cardSettings.tooltipOffset;
            else return cardSettings.tooltipOffset;
        }

        return Vector3.zero;
    }
}
