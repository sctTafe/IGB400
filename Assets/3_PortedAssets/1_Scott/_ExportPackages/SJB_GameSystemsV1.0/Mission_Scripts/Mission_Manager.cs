using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;


namespace Scott.Barley.v2
{
    public class Mission_Manager : MonoBehaviour
    {

        //[SerializeField] List<Transform> missionTargetTrasfroms_List;

        [SerializeField] Transform ms2_Armoury_Transfrom;
        [SerializeField] Transform ms2_ArmouryLandingPad_Transfrom;

        private MiniMapPointerRings miniMapPointerRings;
        [SerializeField] int current_MissionStageID;


        private void Start()
        {
            miniMapPointerRings = FindObjectOfType<MiniMapPointerRings>();
        }
        public void fnc_Trigger_Stage_1()
        {
            // (1) Message Player wi
        }



        public void fnc_MissionManager_Trigger(int missionStageID)
        {
            if (missionStageID == 1)
            {
                miniMapPointerRings.fnc_SetRing2_TransformnTarget(ms2_Armoury_Transfrom);
                if (missionStageID > current_MissionStageID) current_MissionStageID = missionStageID;
            }

            if (missionStageID == 2)
            {
                miniMapPointerRings.fnc_SetRing2_TransformnTarget(ms2_ArmouryLandingPad_Transfrom);
                if (missionStageID > current_MissionStageID) current_MissionStageID = missionStageID;
            }


        }

        // Stage 1      - Get to Armoury
        // Stage 2      - Get to Armoury LandingPad
        // Stage 3   
        // Stage 4




    }
}
