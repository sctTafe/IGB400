using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Description: This script wiil remove the fog from the camera linked
/// 
/// Source: https://answers.unity.com/questions/585469/disabling-fog-for-1-camera.html
/// </summary>
public class NoFog : MonoBehaviour
{
    public Camera cameraMinusFog;

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }
    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == cameraMinusFog)
            RenderSettings.fog = false;
        else
            RenderSettings.fog = true;
    }
}
