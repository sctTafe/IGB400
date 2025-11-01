using TMPro;
using UnityEngine;

public class UI_SpeedBar_Singelton : Singleton<UI_SpeedBar_Singelton>
{
    [SerializeField] UI_SliderOutputControl _Speed_SliderOutputControl;
    [SerializeField] TMP_Text _value_txt;
    [SerializeField] bool _startFull = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float percentage;

        if (_startFull)
            percentage = 1f;
        else
            percentage = 0f;

        percentage = Mathf.Min(percentage, 1); // Cap at 100%
        _Speed_SliderOutputControl?.fn_SetFillPct_NoLerp(percentage);
        
        if (_value_txt != null)
            _value_txt.text = (percentage * 100f).ToString("F0");
    }

    public void fn_SetValue(float value, bool withLeap = false, float rawValue = 0f)
    {
        if (value < 0)
            value = 0;

        float percentage = Mathf.Min(value, 1); // Cap at 100%

        if (withLeap)
        {
            _Speed_SliderOutputControl?.fn_SetFillPct_Lerp(percentage);
        }
        else
        {
            _Speed_SliderOutputControl?.fn_SetFillPct_NoLerp(percentage);
        }


        if (_value_txt != null)
        {
            if (rawValue != 0f)
                _value_txt.text = rawValue.ToString("F0");
            else
                _value_txt.text = (percentage * 100f).ToString("F0");
        }

    }

}
