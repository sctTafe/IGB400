using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ButtonSpriteSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Button Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite pressedSprite;

    private Image buttonImage;
    private bool isHovering = false;
    private bool isPressed = false;

    void Awake()
    {
        buttonImage = GetComponent<Image>();

        if (buttonImage == null)
        {
            Debug.LogError("ButtonSpriteSwitcher requires an Image component on the same GameObject!");
            return;
        }

        // Set initial sprite
        if (normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateSprite();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateSprite();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        UpdateSprite();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (buttonImage == null) return;

        // Priority: Pressed > Hover > Normal
        if (isPressed && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
        }
        else if (isHovering && hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
        }
        else if (normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }
}