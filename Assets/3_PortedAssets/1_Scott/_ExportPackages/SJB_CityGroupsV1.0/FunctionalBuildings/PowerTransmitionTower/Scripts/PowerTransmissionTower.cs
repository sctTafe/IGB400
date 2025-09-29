using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Scott.Barley.v2;


namespace Scott.Barley.v2
{
    public class PowerTransmissionTower : MonoBehaviour
    {

        
        [SerializeField] string PlayerTag;
        [SerializeField] int towerEffectRadius = 500;
        [SerializeField] int towerBatteryReserveCapacity = 500;
        [SerializeField] float maxDefultRechargeRate_PerSecond;
        [SerializeField] bool towerIsUsingReservePower;

        [SerializeField] UnityEvent updateEnergy_Event;

        private Transform playerTransfrom;
        private Transform powerTowerTransfrom;
        private float modifiedRechargeRate;
        private float maxDefultRechargeRate_4Hz;
        private float quater_maxDefultRechargeRate_4Hz;

        private float waitTillTime;


        private Player_Stats player_Stats;
        private void Start()
        {
            player_Stats = FindObjectOfType<Player_Stats>();  //Assumes the is only a single version of this!!
            setVariables();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                playerTransfrom = other.transform;

                if (waitTillTime <= Time.time)
                {
                    player_Stats.fnc_changeEnergy(calculated_RechargeValue());
                    updateEnergy_Event?.Invoke();
                    wait();
                }
            }
        }

        private void setVariables()
        {
            powerTowerTransfrom = this.transform;
            maxDefultRechargeRate_4Hz = maxDefultRechargeRate_PerSecond / 4f;
            quater_maxDefultRechargeRate_4Hz = maxDefultRechargeRate_4Hz / 4f;
        }

        private float calculated_RechargeValue(){

            // 1/4 at the edge, ramp the rest to the center.
            float distance_toTowerCenter = Vector3.Distance(playerTransfrom.position, powerTowerTransfrom.position);
            float percentageDistanceToTower = 1 - (distance_toTowerCenter / towerEffectRadius);
            float rechargeRate_AtTower = quater_maxDefultRechargeRate_4Hz * 3;
            float calculatedRechargeRate = quater_maxDefultRechargeRate_4Hz + Mathf.Lerp(0, rechargeRate_AtTower, percentageDistanceToTower);
            return calculatedRechargeRate;
        }


        private void wait()
        {
            waitTillTime = Time.time + 0.25f;
        }




    }

}