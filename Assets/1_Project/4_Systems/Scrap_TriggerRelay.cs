using UnityEngine;
/// <summary>
/// This Script Provided a relay to the singleton, mostly just attached for the sake of remembering which systems are linked together
/// </summary>

public class Scrap_TriggerRelay : MonoBehaviour
{
    public void fn_AddScore(int value) => Score_Singelton.Instance.fn_AddToScrap(value);
}
