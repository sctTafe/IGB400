using UnityEngine;
using Scott.Barley.v2;
using Unity.Cinemachine;

namespace Scott.Barley.v2
{

    public class PlayerCamZoomBasedOnPlayerMomentum : MonoBehaviour
    {

        [Header("Set Up")]
        [SerializeField] CinemachineVirtualCamera cm_virtualCam;
        [SerializeField] float min_zoomLevel = 40;
        [SerializeField] float max_zoomLevel = 140;
        [SerializeField] float max_Zoom_PlayerVelocityMagnitude = 20;
        [SerializeField] float zoomTransitionSpeed = 20;
        [SerializeField] GameObject player_gameObject;
        [Header("Debug Values Out")]
        [SerializeField] bool debugON = false;
        // for debugging and fine tuning
        public float current_playerVelocity;
        public float current_playerVelocity_pct;
        public float current_targetzoom;
        public float current_Actualzoom;


        private CinemachineFramingTransposer cm_frameTransposer;
        private Rigidbody player_rb;


        private float pct_PlayerVelocity;
        private float target_zoom;
        private float current_zoom;


        private void Start()
        {
            cm_frameTransposer = cm_virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            player_rb = player_gameObject.GetComponent<Rigidbody>();
            current_zoom = 60f;
        }

        private void FixedUpdate()
        {
            Check_SpeedPercentageOfPlayer();
            Action_SetTargetZoom();
            Action_ChangeCurrentZoomValue();
            Action_UpdateCinemachineFrameTransposer();
            if (debugON) Debug_OutputcurrentValuesToUnityInspector();
        }


        private void Check_SpeedPercentageOfPlayer()
        {
            pct_PlayerVelocity = Mathf.Clamp(((player_rb.linearVelocity.magnitude / max_Zoom_PlayerVelocityMagnitude)), 0, 1);
        }

        private void Action_SetTargetZoom()
        {
            target_zoom = Mathf.Lerp(min_zoomLevel, max_zoomLevel, pct_PlayerVelocity);
        }

        private void Action_ChangeCurrentZoomValue()
        {
            if (target_zoom < current_zoom)
            {
                current_zoom -= zoomTransitionSpeed * Time.deltaTime;
            }
            else
            {
                if (target_zoom > current_zoom)
                {
                    current_zoom += zoomTransitionSpeed * Time.deltaTime;
                }
            }
        }

        private void Action_UpdateCinemachineFrameTransposer()
        {
            cm_frameTransposer.m_CameraDistance = current_zoom;
        }

        private void Debug_OutputcurrentValuesToUnityInspector()
        {
            current_playerVelocity = player_rb.linearVelocity.magnitude;
            current_playerVelocity_pct = pct_PlayerVelocity;
            current_targetzoom = target_zoom;
            current_Actualzoom = current_zoom;
        }

    }
}