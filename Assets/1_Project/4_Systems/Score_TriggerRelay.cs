using UnityEngine;
/// <summary>
/// This Script Provided a relay to the singleton, mostly just attached for the sake of remembering which systems are linked together
/// </summary>
public class Score_TriggerRelay : MonoBehaviour
{
    public void fn_AddScore(int value) => UI_Score_Singelton.Instance.fn_AddToScore(value);
}
