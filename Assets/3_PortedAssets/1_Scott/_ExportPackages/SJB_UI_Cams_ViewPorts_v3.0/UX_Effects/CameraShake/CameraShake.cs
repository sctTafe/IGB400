using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// NOTE: using outdated CM2 code, probably needs updating
/// </summary>
public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera virtualCamera; //reference the camera
    private float timerForShake;

    private void Awake()
    {

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void CameraShaker(float amplitudeOfCameraShake, float timer)
    {
        CinemachineBasicMultiChannelPerlin cinemachineNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //get the noise component of the virtual camera and set it to cinemachineNoise
        cinemachineNoise.AmplitudeGain = amplitudeOfCameraShake; //set the amplitude of cinemachine noise to amplitudeOfCameraShake float
        cinemachineNoise.FrequencyGain = 1f;
        timerForShake = timer;
    }

    private void Update()
    {
        if (timerForShake > 0)
        {
            timerForShake -= Time.deltaTime;
            if (timerForShake <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineNoise.AmplitudeGain = 0f;
                cinemachineNoise.FrequencyGain = 0f;


            }
        }
    }
}
