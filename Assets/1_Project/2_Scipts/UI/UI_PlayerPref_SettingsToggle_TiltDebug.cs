using UnityEngine;

public class UI_PlayerPref_SettingsToggle_TiltDebug : UI_PlayerPref_SettingsToggle
{

    public override void fn_OnStart()
    {
        base.fn_OnStart();

        UpdateTiltDebugCanvas();
    }

    public override void fn_ToggleSetting()
    {
        base.fn_ToggleSetting();

        UpdateTiltDebugCanvas();
    }


    void UpdateTiltDebugCanvas()
    {
        if (currentValue == true)
        {
            TiltCanvasManager_Singelton.Instance.fn_EnableCanvas(true);
        }
        else
        {
            TiltCanvasManager_Singelton.Instance.fn_EnableCanvas(false);
        }
    }
}
