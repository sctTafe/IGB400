using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UI_UpgradeOption : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;

    [Header("Upgrade SO")]
    [SerializeField] UpgradeActionSO _UpgradeActionSO;

    public UpgradeActionSO UpgradeActionSO => _UpgradeActionSO;

    /// <summary>
    /// Populates the UI elements using data from the attached UpgradeActionSO.
    /// </summary>
    public void ApplyUpgradeData()
    {
        if (_UpgradeActionSO == null)
        {
            Debug.LogError("No UpgradeActionSO assigned.", this);
            return;
        }

        if (upgradeNameText) upgradeNameText.text = _UpgradeActionSO._upgradeName;
        if (descriptionText) descriptionText.text = _UpgradeActionSO._description;
        if (iconImage && _UpgradeActionSO._icon) iconImage.sprite = _UpgradeActionSO._icon;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(UI_UpgradeOption))]
public class UpgradeDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UI_UpgradeOption display = (UI_UpgradeOption)target;

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Apply Upgrade Data"))
        {
            display.ApplyUpgradeData();
        }
    }
}
#endif