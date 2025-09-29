using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Scott.Barley.v2;


namespace Scott.Barley.v2
{

    public class Player_Regen : MonoBehaviour
    {
        [SerializeField] string PlayerTag;

        [SerializeField]  float energyRegenRatePerSecond;
        [SerializeField]  float healthRegenRatePerSecond;

        [SerializeField]  UnityEvent<float> updateHealth_Event;
        [SerializeField]  UnityEvent<float> updateEnergy_Event;

        private float quater_energyRegenRatePerSecond;
        private float quater_healthRegenRatePerSecond;

        private float waitTillTime;

        private void Start()
        {
            quater_energyRegenRatePerSecond = energyRegenRatePerSecond / 4f;
            quater_healthRegenRatePerSecond = healthRegenRatePerSecond / 4f;
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(PlayerTag)){

                if(waitTillTime<= Time.time)
                {
                    updateHealth_Event?.Invoke(quater_healthRegenRatePerSecond);
                    updateEnergy_Event?.Invoke(quater_energyRegenRatePerSecond);
                    wait();
                }
            }
        }

        private void wait()
        {
            waitTillTime = Time.time + 0.25f;
        }


    }
}