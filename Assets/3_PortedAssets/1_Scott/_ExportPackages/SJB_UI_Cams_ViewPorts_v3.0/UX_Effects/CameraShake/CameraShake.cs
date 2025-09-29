//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;

///// <summary>
///// Author: David Deasy
///// Description: This script will control the camera shake effect 
///// Source: https://www.youtube.com/watch?v=ACf1I27I6Tk
///// </summary>
//public class CameraShake : MonoBehaviour
//{
    
//    private CinemachineVirtualCamera virtualCamera; //reference the camera
//    private float timerForShake;

//    private void Awake()
//    {
        
//        virtualCamera = GetComponent<CinemachineVirtualCamera>();
//    }

//    public void CameraShaker(float amplitudeOfCameraShake, float timer)
//    {
//        CinemachineBasicMultiChannelPerlin cinemachineNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //get the noise component of the virtual camera and set it to cinemachineNoise
//        cinemachineNoise.m_AmplitudeGain = amplitudeOfCameraShake; //set the amplitude of cinemachine noise to amplitudeOfCameraShake float
//        cinemachineNoise.m_FrequencyGain = 1f;
//        timerForShake = timer;
//    }

//    private void Update()
//    {
//        if (timerForShake > 0)
//        {
//            timerForShake -= Time.deltaTime;
//            if(timerForShake <= 0f)
//            {
//                CinemachineBasicMultiChannelPerlin cinemachineNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
//                cinemachineNoise.m_AmplitudeGain = 0f;
//                cinemachineNoise.m_FrequencyGain = 0f;


//            }
//        }
//    }

//    //
//}
