using UnityEngine;

public class TiltCanvasManager_Singelton : Singleton<TiltCanvasManager_Singelton>
{ 
    public Canvas canvas;

    public void fn_ToggleCanvas()
    {
        if (canvas != null)
        {
            canvas.enabled = !canvas.enabled;
        }
    }

    public void fn_EnableCanvas(bool isTrue)
    {
        if (canvas != null)
        {
            if (isTrue)
            {
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
            }
        }
    }
}
