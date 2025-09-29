using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class Mission_Armoury_EnableWeapons : MonoBehaviour
    {

        private Weapons_AddAndClear_OnDeathAndTrigger weapons_AddAndClear_OnDeathAndTrigger;
        [SerializeField] string PlayerTag = "Player";

        private void Start()
        {
            ConnectToScript();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                if (weapons_AddAndClear_OnDeathAndTrigger == null) ConnectToScript();
                weapons_AddAndClear_OnDeathAndTrigger.fnc_AddWeaponsToPlayer();
            }

        }

        private void ConnectToScript()
        {
            weapons_AddAndClear_OnDeathAndTrigger = FindObjectOfType<Weapons_AddAndClear_OnDeathAndTrigger>();
        }




    }
}