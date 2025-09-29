using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scott.Barley.v2
{

    public class MiniMapPointerRings : MonoBehaviour
    {
        [SerializeField] float disableRingWhenClose_Distance;

        [SerializeField] Transform ring1_TargetTransform_Power;
        [SerializeField] Transform ring2_TargetTransform_Mission;
        [SerializeField] Transform ring3_TargetTransform_Home;

        [SerializeField] Transform ring1_Transform;
        [SerializeField] Transform ring2_Transform;
        [SerializeField] Transform ring3_Transform;

        [SerializeField] bool ring1_IsEnabled;
        [SerializeField] bool ring2_IsEnabled;
        [SerializeField] bool ring3_IsEnabled;

        [SerializeField] float pointerRingsScalingFactor;


        private void Start()
        {
            SetUp_turnAllRingsOn();
        }

        private void FixedUpdate()
        {
            CheckforNull_TargetTransfroms();
            SetRings_ActiveInactive();
            RotateRingsToTargets();
        }


        private void SetUp_turnAllRingsOn()
        {
            ring1_IsEnabled = true;
            ring2_IsEnabled = true;
            ring3_IsEnabled = true;
        }
        private void CheckforNull_TargetTransfroms()
        {
            if(ring1_TargetTransform_Power == null)
            {
                ring1_IsEnabled = false;
            }
            if (ring2_TargetTransform_Mission == null)
            {
                ring2_IsEnabled = false;
            }
            if (ring3_TargetTransform_Home == null)
            {
                ring3_IsEnabled = false;
            }
        }
        private void SetRings_ActiveInactive()
        {
            if (ring1_IsEnabled)
            {
                if (!ring1_Transform.gameObject.activeSelf)
                {
                    ring1_Transform.gameObject.SetActive(true);
                }
            }
            else
            {
                if (ring1_Transform.gameObject.activeSelf)
                {
                    ring1_Transform.gameObject.SetActive(false);
                }
            }

            if (ring2_IsEnabled)
            {
                if (!ring2_Transform.gameObject.activeSelf)
                {
                    ring2_Transform.gameObject.SetActive(true);
                }
            }
            else
            {
                if (ring2_Transform.gameObject.activeSelf)
                {
                    ring2_Transform.gameObject.SetActive(false);
                }
            }

            if (ring3_IsEnabled)
            {
                if (!ring3_Transform.gameObject.activeSelf)
                {
                    ring3_Transform.gameObject.SetActive(true);
                }
            }
            else
            {
                if (ring3_Transform.gameObject.activeSelf)
                {
                    ring3_Transform.gameObject.SetActive(false);
                }
            }
        }
        private void RotateRingsToTargets()
        {
            if (ring1_IsEnabled)
            {
                ring1_Transform.LookAt(ring1_TargetTransform_Power);
                ring1_Transform.eulerAngles = new Vector3(0, ring1_Transform.eulerAngles.y, 0);
            }
            if (ring2_IsEnabled)
            {
                ring2_Transform.LookAt(ring2_TargetTransform_Mission);
                ring2_Transform.eulerAngles = new Vector3(0, ring2_Transform.eulerAngles.y, 0);
            }
            if (ring3_IsEnabled)
            {
                ring3_Transform.LookAt(ring3_TargetTransform_Home);
                ring3_Transform.eulerAngles = new Vector3(0, ring3_Transform.eulerAngles.y, 0);
            }
        }


        public void fnc_SetRing1_TransformnTarget(Transform target)
        {
            ring1_TargetTransform_Power = target;
            ring1_IsEnabled = true;
        }
        public void fnc_SetRing2_TransformnTarget(Transform target)
        {
            ring2_TargetTransform_Mission = target;
            ring2_IsEnabled = true;
        }
        public void fnc_SetRing3_TransformnTarget(Transform target)
        {
            ring3_TargetTransform_Home = target;
            ring3_IsEnabled = true;
        }
    }
}
