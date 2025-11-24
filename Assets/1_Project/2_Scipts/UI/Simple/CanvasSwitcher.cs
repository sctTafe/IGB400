using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    [Header("UI References")]
    public Button triggerButton;
    public Canvas canvasToDisable;
    public Canvas canvasToEnable;

    private void Start()
    {
        StartState();

        if (triggerButton != null)
            triggerButton.onClick.AddListener(SwitchCanvas);
        else
            Debug.LogWarning($"{name}: No trigger button assigned!");
    }

    private void OnDestroy()
    {
        // Clean up listener to avoid leaks
        if (triggerButton != null)
            triggerButton.onClick.RemoveListener(SwitchCanvas);
    }

    private void SwitchCanvas()
    {
        if (canvasToDisable != null)
            canvasToDisable.gameObject.SetActive(false);

        if (canvasToEnable != null)
            canvasToEnable.gameObject.SetActive(true);
    }


    void StartState()
    {
        if (canvasToDisable != null)
            canvasToDisable.gameObject.SetActive(true);

        if (canvasToEnable != null)
            canvasToEnable.gameObject.SetActive(false);
    }
}
