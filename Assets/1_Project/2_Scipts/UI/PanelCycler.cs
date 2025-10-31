using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelCycler : MonoBehaviour
{
    [Header("Panel Settings")]
    [Tooltip("Add all your panels here in the order you want them to appear")]
    public List<GameObject> panels = new List<GameObject>();

    [Header("Button References")]
    public Button leftButton;
    public Button rightButton;

    private int currentPanelIndex = 0;

    void Start()
    {
        // Subscribe to button click events
        if (leftButton != null)
            leftButton.onClick.AddListener(ShowPreviousPanel);

        if (rightButton != null)
            rightButton.onClick.AddListener(ShowNextPanel);

        // Show the first panel at start
        ShowPanel(currentPanelIndex);
    }

    void ShowPanel(int index)
    {
        // Deactivate all panels
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        // Activate the selected panel
        if (index >= 0 && index < panels.Count && panels[index] != null)
        {
            panels[index].SetActive(true);
            currentPanelIndex = index;
        }
    }

    public void ShowNextPanel()
    {
        if (panels.Count == 0) return;

        currentPanelIndex++;

        // Loop back to the first panel
        if (currentPanelIndex >= panels.Count)
            currentPanelIndex = 0;

        ShowPanel(currentPanelIndex);
    }

    public void ShowPreviousPanel()
    {
        if (panels.Count == 0) return;

        currentPanelIndex--;

        // Loop back to the last panel
        if (currentPanelIndex < 0)
            currentPanelIndex = panels.Count - 1;

        ShowPanel(currentPanelIndex);
    }

    // Optional: Jump to a specific panel by index
    public void ShowPanelByIndex(int index)
    {
        if (index >= 0 && index < panels.Count)
        {
            ShowPanel(index);
        }
    }

    void OnDestroy()
    {
        // Clean up button listeners
        if (leftButton != null)
            leftButton.onClick.RemoveListener(ShowPreviousPanel);

        if (rightButton != null)
            rightButton.onClick.RemoveListener(ShowNextPanel);
    }
}