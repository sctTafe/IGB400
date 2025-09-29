using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;


namespace Scott.Barley.v2
{
    public class Mission_Trigger_OnStayCollider : MonoBehaviour
    {
        [SerializeField] string PlayerTag;
        [SerializeField] int missionStage_ID;
        private Mission_Manager mission_Manager;

        private bool isTriggered;
        void Start()
        {
            mission_Manager = FindObjectOfType<Mission_Manager>();  //Assumes the is only a single version of this!!
            isTriggered = false;
        }



        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                if (!isTriggered)
                {
                    mission_Manager.fnc_MissionManager_Trigger(missionStage_ID);
                    isTriggered = true;
                }
            }
        }

    }
}