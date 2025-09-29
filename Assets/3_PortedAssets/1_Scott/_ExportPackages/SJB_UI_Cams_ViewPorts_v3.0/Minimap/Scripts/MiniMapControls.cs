//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Author: David Deasy
///// Modified By: Scott Barley
///// Description: This script will control the movement of the minimap virtual camera to follow the player
///// Minimap Camera must not be parented to the player as this is not stable
///// Source: https://www.youtube.com/watch?v=28JTTXqMvOU
///// </summary>
//public class MiniMapControls : MonoBehaviour
//{
//    //Class Variables
//    [SerializeField] Transform targetTransform;
//    [SerializeField] Transform virtualCamTransform;
//    [SerializeField] Cinemachine.CinemachineVirtualCamera minimapCamera;
//    [SerializeField] float defaultOrthographicSize = 100f;

//    private Vector3 newVirtualCamPosition;
//    private bool lockMiniMapRotation;

    

//    //need to do this in late update so that it will update the position of the minimap after the player has moved
//    void LateUpdate()
//    {
//        MoveVirtualCamXZValues();
//        RotateVirtualCam();
//    }

//    // ---- Private Functions ----
//    private void MoveVirtualCamXZValues()
//    {
//        newVirtualCamPosition = new Vector3(targetTransform.position.x, virtualCamTransform.position.y, targetTransform.position.z);
//        virtualCamTransform.position = newVirtualCamPosition;
//    }

//    private void RotateVirtualCam()
//    {
//        if (lockMiniMapRotation)
//        {
//            //uses the players rotation in the y axis - this is better than parenting
//            virtualCamTransform.rotation = Quaternion.Euler(90f, targetTransform.eulerAngles.y, 0f);
//        }
//    }

//    // ---- Public Functions ----
//    public void btn_ToggleLockMinimapRotation()
//    {
//        lockMiniMapRotation = !lockMiniMapRotation;
//        Debug.Log("MiniMapControls: lockMiniMapRotation: " + lockMiniMapRotation);
//    }
//    public void sldr_ChangeVIrtualCamOthorgraphicSize(System.Single sliderValue)
//    {
//        minimapCamera.m_Lens.OrthographicSize = defaultOrthographicSize * sliderValue;
//    }

   
//}