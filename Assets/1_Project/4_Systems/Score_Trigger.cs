using UnityEngine;

public class Score_Trigger : MonoBehaviour
{
    public void fn_AddScore(int value) => Score_Singelton.Instance.fn_AddToScore(value);
}
