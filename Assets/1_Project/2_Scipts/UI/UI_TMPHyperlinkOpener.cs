using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// /
///  EXAMPLE: in the TMP txt box => Visit our <link="https://www.example.com">website</link> for more info!
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class UI_TMPHyperlinkOpener : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // OLD INPUT SYSTEM:
        // int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);

        // NEW INPUT SYSTEM:
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, eventData.pressEventCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
