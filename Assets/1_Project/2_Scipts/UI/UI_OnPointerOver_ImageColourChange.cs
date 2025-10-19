using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_OnPointerOver_ImageColourChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Image References")]
    [SerializeField] private Image[] targetImages; // Assign your images here

    [Header("Colour Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;

    // Called when mouse enters the area
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetImageColors(hoverColor);
    }

    // Called when mouse exits the area
    public void OnPointerExit(PointerEventData eventData)
    {
        SetImageColors(normalColor);
    }

    private void SetImageColors(Color color)
    {
        if (targetImages == null) return;

        foreach (var image in targetImages)
        {
            if (image != null)
            {
                // Preserve the current alpha value
                Color newColor = color;
                newColor.a = image.color.a;
                image.color = newColor;
            }
        }
    }
}