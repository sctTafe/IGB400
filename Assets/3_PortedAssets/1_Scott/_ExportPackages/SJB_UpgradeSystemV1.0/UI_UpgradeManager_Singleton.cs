using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeManager_Singleton : Singleton<UI_UpgradeManager_Singleton>
{
    [Header("UI References")]
    public GameObject upgradeCanvas;                    // The entire canvas or panel to enable/disable
    public GameObject[] upgradeOptions;                 // All upgrade panels (child panels with buttons)
    private Canvas _canvas;

    [Header("Upgrade Settings")]
    public int numberOfUpgradesToShow = 3;              // Number of options to show
    //public int pointsRequiredToTrigger = 100;           // Example condition

    [Header("Player")]
    public int playerPoints = 0;                        // Example player points (update based on your game)

    private List<GameObject> activeUpgrades = new List<GameObject>();

 
    void Start()
    {
        _canvas = upgradeCanvas.GetComponent<Canvas>();
        _canvas.enabled = false;
        //upgradeCanvas.SetActive(false);

        // Disable all upgrade panels at start
        foreach (var panel in upgradeOptions)
        {
            panel.SetActive(false);
        }

        // Add button listeners
        foreach (var panel in upgradeOptions)
        {
            Button btn = panel.GetComponentInChildren<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnUpgradeSelected(panel));
            }
        }
    }

    public void fn_CallUpgrade()
    {

        ShowUpgradePanel();
        Time.timeScale = 0f; // Pauses the game
    } 

    void ShowUpgradePanel()
    {
        // Clear previous active list
        activeUpgrades.Clear();

        // Randomly select upgrades
        List<int> indices = new List<int>();
        while (indices.Count < numberOfUpgradesToShow)
        {
            int rand = Random.Range(0, upgradeOptions.Length);
            if (!indices.Contains(rand))
                indices.Add(rand);
        }

        // Activate selected upgrade panels
        foreach (int i in indices)
        {
            upgradeOptions[i].SetActive(true);
            activeUpgrades.Add(upgradeOptions[i]);
        }

        // Show canvas
        //upgradeCanvas.SetActive(true);
        _canvas.enabled = true;
    }

    void OnUpgradeSelected(GameObject selectedPanel)
    {
        Debug.Log("Upgrade Selected: " + selectedPanel.name);

        // Call upgrade logic here
        ApplyUpgrade(selectedPanel);

        // Close upgrade panel and reset
        HideUpgradePanel();
    }

    void ApplyUpgrade(GameObject panel)
    {
        // TODO: Fill this in based on which panel was clicked
        // Could use a component or script on the panel to identify what upgrade it represents
        Debug.Log("Applying upgrade logic for: " + panel.name);
        Time.timeScale = 1f; // Resumes the game
    }

    void HideUpgradePanel()
    {
        // Hide all upgrade options
        foreach (var panel in activeUpgrades)
        {
            panel.SetActive(false);
        }

        activeUpgrades.Clear();
        //upgradeCanvas.SetActive(false);
        _canvas.enabled = false;
    }

    [ContextMenu("Toggle Canvas")]
    private void ToggleCanvas()
    {
        ShowUpgradePanel();
    }
}